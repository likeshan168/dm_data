using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.BZip2;
using ICSharpCode.SharpZipLib.GZip;
using ICSharpCode.SharpZipLib.Zip;


namespace WindowsFormsApplication3
{
    class ZipCompress
    {

        
        public static byte[] Compress(byte[] bytesToCompress)
        {

           

            byte[] rebyte = null;
            MemoryStream ms = new MemoryStream();
            //ZipOutputStream s = new ZipOutputStream(ms);
            // ZipEntry ZipEntry = new ZipEntry("ZippedFile");
            //ZipEntry.CompressionMethod = CompressionMethod.Deflated;
            //s.PutNextEntry(ZipEntry);

            GZipOutputStream s = new GZipOutputStream(ms);
            s.Write(bytesToCompress, 0, bytesToCompress.Length);
            s.Close();
            rebyte = ms.ToArray();
           
            ms.Close();
            return rebyte;
            //return ms.ToArray();
        }

        private static string Compress(string stringToCompress)
        {
            byte[] compressedData = CompressToByte(stringToCompress);
            string strOut = Convert.ToBase64String(compressedData);
            return strOut;
        }

        public static byte[] CompressToByte(string stringToCompress)
        {
            byte[] bytData = Encoding.Default.GetBytes(stringToCompress);
            return Compress(bytData);
        }
              
        //解压缩
        public static byte[] DeCompress(byte[] data)
        {
            int orginalLen = data.Length;
            int maxDecompressLen = 20 * orginalLen;

            if (maxDecompressLen < 100000) //缓冲区最小100K,最大8M,原始数据如果大于25KB，则解压缓冲为20倍原始数据大小
            {
                maxDecompressLen = 100000;
            }
            if (maxDecompressLen > 8000000)
            {
                maxDecompressLen = 8000000;
            }
            byte[] decompressByteArr = new byte[maxDecompressLen];
            //int len = 0;
            int read = -1;
            try
            {
             
               // MemoryStream inStream = new MemoryStream(data);
                //ZipInputStream s2 = new ZipInputStream(new MemoryStream(data));
                //s2.GetNextEntry();

                GZipInputStream s2 = new GZipInputStream(new MemoryStream(data));
                MemoryStream outStream = new MemoryStream();
              
                read = s2.Read(decompressByteArr, 0, decompressByteArr.Length);
                while (read > 0)
                {
                    outStream.Write(decompressByteArr, 0, read);
                    read = s2.Read(decompressByteArr, 0, decompressByteArr.Length);
                }
                s2.Close();
                return outStream.ToArray();
                //int haveRead = 0;
                //int count;
                // while ((count = s2.Read(decompressByteArr, haveRead, 1024)) >= 0)
                //{
                //    haveRead += count;
                //}
                //len = haveRead;
                ////while (true)
                ////{
                ////    int size = s2.Read(writeData, 0, writeData.Length);
                ////    if (size > 0)
                ////    {
                ////        outStream.Write(writeData, 0, size);
                ////    }
                ////    else
                ////    {
                ////        break;
                ////    }
                ////}
                //s2.Close();
                ////byte[] outArr = outStream.ToArray();
                ////outStream.Close();
                //return decompressByteArr;
            }
            catch (Exception ex)
            {
                //ex.printStackTrace();
                ErrInfo.WriterErrInfo("ZipCompress", "DeCompress", ex);
            }

            return decompressByteArr;
        }


         

    }
}
