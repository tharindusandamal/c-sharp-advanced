using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace ImageSample
{
    class Program
    {
        static void Main(string[] args)
        {
            string p1 = @"D:\ENADOC\Sample docs\TIFF\Sample.tiff";
            string p2 = @"D:\ENADOC\Sample docs\TIFF\Sample2.tiff";
            File.WriteAllBytes(p2, SplitTiff(File.ReadAllBytes(p1), new int[] { 1 }));
        }

        private static byte[] GetNewFile(string path, int[] pages)
        {
            // new image
            Image source = null;
            //Get the frame dimension list from the image of the file and 
            Image tiffImage = Image.FromFile(path);
            //get the globally unique identifier (GUID) 
            Guid objGuid = tiffImage.FrameDimensionsList[0];
            //create the frame dimension 
            FrameDimension dimension = new FrameDimension(objGuid);
            //Gets the total number of frames in the .tiff file 
            int noOfPages = tiffImage.GetFrameCount(dimension);

            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Compression;
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, (long)EncoderValue.CompressionNone);

            var encoderParameters = new EncoderParameters(2);
            encoderParameters.Param[0] = myEncoderParameter;

            var encoderInfo = ImageCodecInfo.GetImageEncoders().First(k => k.MimeType == "image/tiff");

            ImageCodecInfo encodeInfo = null;
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < imageEncoders.Length; j++)
            {
                if (imageEncoders[j].MimeType == "image/tiff")
                {
                    encodeInfo = imageEncoders[j];
                    break;
                }
            }

            foreach (Guid guid in tiffImage.FrameDimensionsList)
            {
                for (int index = 0; index < noOfPages; index++)
                {
                    if (pages.Any(p => p == index))
                    {
                        FrameDimension currentFrame = new FrameDimension(guid);
                        tiffImage.SelectActiveFrame(currentFrame, (index));
                        using (MemoryStream mStream = new MemoryStream())
                        {
                            tiffImage.Save(mStream, tiffImage.RawFormat);

                            if (source == null)
                            {
                                encoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                                source = new Bitmap(Image.FromStream(mStream));
                                source.Save(new MemoryStream(), encoderInfo, encoderParameters);
                            }
                            else
                            {
                                encoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                                source.SaveAdd(Image.FromStream(mStream), encoderParameters);
                            }
                        }
                    }
                }
            }

            if (source != null)
            {
                EncoderParameter SaveEncodeParam = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.Flush);
                encoderParameters = new EncoderParameters(1);
                encoderParameters.Param[0] = SaveEncodeParam;
                source.SaveAdd(encoderParameters);

                using (MemoryStream stream = new MemoryStream())
                {
                    source.Save(stream, ImageFormat.Tiff);
                    return stream.ToArray();
                }
            }
            else
                return null;
        }

        private static void Split(string pstrInputFilePath, string pstrOutputPath)
        {
            //Get the frame dimension list from the image of the file and 
            Image tiffImage = Image.FromFile(pstrInputFilePath);
            //get the globally unique identifier (GUID) 
            Guid objGuid = tiffImage.FrameDimensionsList[0];
            //create the frame dimension 
            FrameDimension dimension = new FrameDimension(objGuid);
            //Gets the total number of frames in the .tiff file 
            int noOfPages = tiffImage.GetFrameCount(dimension);

            ImageCodecInfo encodeInfo = null;
            ImageCodecInfo[] imageEncoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < imageEncoders.Length; j++)
            {
                if (imageEncoders[j].MimeType == "image/tiff")
                {
                    encodeInfo = imageEncoders[j];
                    break;
                }
            }

            // Save the tiff file in the output directory. 
            if (!Directory.Exists(pstrOutputPath))
                Directory.CreateDirectory(pstrOutputPath);

            foreach (Guid guid in tiffImage.FrameDimensionsList)
            {
                for (int index = 0; index < noOfPages; index++)
                {
                    FrameDimension currentFrame = new FrameDimension(guid);
                    tiffImage.SelectActiveFrame(currentFrame, index);
                    tiffImage.Save(string.Concat(pstrOutputPath, @"\", index, ".TIF"), encodeInfo, null);
                }
            }
        }

        /// <summary> 
        /// This method reads the value of each metadata property of the tiff file 
        /// </summary> 
        /// <param name="pstrFilePath"></param> 
        private void ReadTiffProperties(string pstrFilePath)
        {
            try
            {
                System.Drawing.Bitmap newImage = new System.Drawing.Bitmap(pstrFilePath);

                // Get the properties collection of the file 
                System.Drawing.Imaging.PropertyItem[] tiffProperties = newImage.PropertyItems;
                PropertyItem currentItem = null;
                object objValue = null;

                for (int i = 0; i < tiffProperties.GetLength(0); i++)
                {
                    currentItem = tiffProperties[i];
                    objValue = ReadPropertyValue(currentItem.Type, currentItem.Value);
                }
                newImage = null;
                tiffProperties = null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        /// <summary> 
        /// This method reads the tiff property based on the property type 
        /// </summary> 
        /// <param name="pItemType"></param> 
        /// <param name="pitemValue"></param> 
        /// <returns></returns> 
        private object ReadPropertyValue(short pItemType, byte[] pitemValue)
        {
            // Read all the properties of the file. 
            object objValue = null;
            System.Text.Encoding asciiEnc = System.Text.Encoding.ASCII;
            // Read the values based on the type of the propery 
            switch (pItemType)
            {
                case 2: // Value is a null-terminated ASCII string.  
                    objValue = asciiEnc.GetString(pitemValue);
                    break;
                case 3: // Value is an array of unsigned short (16-bit) integers. 
                    objValue = System.BitConverter.ToUInt16(pitemValue, 0);
                    break;
                case 4: // Value is an array of unsigned long (32-bit) integers. 
                    objValue = System.BitConverter.ToUInt32(pitemValue, 0);
                    break;
                default:
                    break;
            }
            return objValue;
        }

        private static byte[] TiffCompress(string SourcePath, int[] pages)
        {
            Image image = null;

            try
            {
                var DocOutputMemStream = new MemoryStream();
                image = System.Drawing.Image.FromFile(SourcePath);
                var PageCount = image.GetFrameCount(FrameDimension.Page);

                //Tiff
                ImageCodecInfo myImageCodecInfo = ImageCodecInfo.GetImageEncoders().First(k => k.MimeType == "image/tiff");
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Compression;
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, (long)EncoderValue.CompressionCCITT4);
                EncoderParameters myEncoderParameters = new EncoderParameters(2);
                myEncoderParameters.Param[0] = myEncoderParameter;
                //End tiff

                for (int index = 1; index < PageCount; index++)
                {
                    if (pages.Any(p => p == index))
                    {
                        image.SelectActiveFrame(FrameDimension.Page, index);

                        //Threshold
                        System.Drawing.Image frame = image;

                        if (index == 1)
                        {
                            // Initialize the first frame of multipage tiff.                    
                            myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                            frame.Save(DocOutputMemStream, myImageCodecInfo, myEncoderParameters);

                        }
                        else
                        {
                            // Add additional frames. 
                            myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                            frame.SaveAdd(frame, myEncoderParameters);

                        }

                        if (index == PageCount - 1)
                        {
                            // When it is the last frame, flush the resources and closing. 
                            myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.Flush);
                            frame.SaveAdd(myEncoderParameters);
                        }
                    }
                }

                //image.Dispose();

                using (var m = new MemoryStream())
                {
                    image.Save(m, ImageFormat.Tiff);
                    return m.ToArray();
                }
            }
            catch (Exception ex)
            {
                image.Dispose();
                return null;
            }

        }

        private static byte[] GetNewTiffDocument(string path, int[] pages)
        {
            try
            {
                Image _baseImage = null;

                return null;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private static void Sample(byte[] document)
        {
            Image img = null;
            MemoryStream memstream = new MemoryStream();
            memstream = new MemoryStream(document);
            Bitmap bitmaptry = (Bitmap)Image.FromStream(memstream);
            List<Image> images = new List<Image>();
            Bitmap bitmap = (Bitmap)Image.FromStream(memstream);
            int count = bitmap.GetFrameCount(FrameDimension.Page);

            //List<Image> printList = new List<Image>();
            //string docName = "TestingDocumentName";
            //File.WriteAllBytes(@"E" + docName + ".TIFF", Document);
            for (int idx = 0; idx < count; idx++)
            {
                //save each frame to a bytestream
                bitmap.SelectActiveFrame(FrameDimension.Page, idx);
                MemoryStream byteStream = new MemoryStream();
                bitmap.Save(byteStream, ImageFormat.Tiff);
                images.Add(Image.FromStream(byteStream));
                img = Image.FromStream(byteStream);

                //using (var ms = new MemoryStream((byte[])(document)))
                //{
                //    printList.Add(Image.FromStream(ms));
                //}
                //***if below line uncommented ,you only can get printouts on Legal Papers only***//
                //---//pd.DefaultPageSettings.PaperSize = new PaperSize("Legal", 850, 1400);
            }
        }

        private static void Test(string path)
        {
            //TiffBitmapDecoder decoder = new TiffBitmapDecoder(new MemoryStream(File.ReadAllBytes(path)), BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
            //int totFrames = decoder.Frames.Count;

            //BitmapEncoder enc = new BmpBitmapEncoder();

            //foreach (var item in decoder.Frames)
            //{
            //    enc.Frames.Add(BitmapFrame.Create());
            //}



            //for (int i = 0; i < totFrames; i++)
            //{
            //    enc.Frames.Add(BitmapFrame.Create());
            //}

            using (var msTemp = new MemoryStream(File.ReadAllBytes(path)))
            {
                TiffBitmapDecoder decoder = new TiffBitmapDecoder(msTemp, BitmapCreateOptions.PreservePixelFormat, BitmapCacheOption.Default);
                int totFrames = decoder.Frames.Count;
                BitmapSource bitmapSource = decoder.Frames[0];

                TiffBitmapEncoder encoder = new TiffBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));


            }










            //System.Drawing.Imaging.Encoder encoder = System.Drawing.Imaging.Encoder.SaveFlag;
            //ImageCodecInfo encoderInfo = ImageCodecInfo.GetImageEncoders().First(i => i.MimeType == "image/tiff");
            //EncoderParameters encoderParameters = new EncoderParameters(1);

            //Image tiffImage = Image.FromFile(path);

            //// Save the first frame of the multi page tiff
            //encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.MultiFrame);
            //Bitmap firstImage = (Bitmap)_scannedPages[0].RawContent;
            //firstImage.Save(fileName, encoderInfo, encoderParameters);

            //// Add the remaining images to the tiff
            //encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.FrameDimensionPage);

            //for (int i = 1; i < _scannedPages.Count; i++)
            //{
            //    Bitmap img = (Bitmap)_scannedPages[i].RawContent;
            //    firstImage.SaveAdd(img, encoderParameters);
            //}

            //// Close out the file
            //encoderParameters.Param[0] = new EncoderParameter(encoder, (long)EncoderValue.Flush);
            //firstImage.SaveAdd(encoderParameters);
        }

        private static void TiffCompress(string SourcePath)
        {
            System.Drawing.Image image = null;
            try
            {
                var DocOutputMemStream = new MemoryStream();
                image = System.Drawing.Image.FromFile(SourcePath);
                var PageCount = image.GetFrameCount(FrameDimension.Page);


                //Tiff            
                ImageCodecInfo myImageCodecInfo;
                System.Drawing.Imaging.Encoder myEncoder;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;

                myImageCodecInfo = ImageCodecInfo.GetImageEncoders().First(k => k.MimeType == "image/tiff");
                myEncoder = System.Drawing.Imaging.Encoder.Compression;
                myEncoderParameters = new EncoderParameters(2);

                myEncoderParameter = new EncoderParameter(myEncoder, (long)EncoderValue.CompressionCCITT4);
                myEncoderParameters.Param[0] = myEncoderParameter;
                //End tiff

                PageCount = 3;

                for (int index = 0; index < PageCount; index++)
                {
                    image.SelectActiveFrame(FrameDimension.Page, index);

                    //Threshold
                    System.Drawing.Image frame = image;

                    if (index == 0)
                    {
                        // Initialize the first frame of multipage tiff.                    
                        myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                        frame.Save(DocOutputMemStream, myImageCodecInfo, myEncoderParameters);

                    }
                    else
                    {
                        // Add additional frames. 
                        myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                        frame.SaveAdd(frame, myEncoderParameters);

                    }

                    if (index == PageCount - 1)
                    {
                        // When it is the last frame, flush the resources and closing. 
                        myEncoderParameters.Param[1] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.Flush);
                        frame.SaveAdd(myEncoderParameters);
                    }
                }

                //image.Save(@"D:\ENADOC\Sample docs\TIFF\Sample2.tiff");
                File.WriteAllBytes(@"D:\ENADOC\Sample docs\TIFF\Sample2.tiff", DocOutputMemStream.ToArray());
                image.Dispose();

            }
            catch (Exception ex)
            {
                
            }
        }

        private static byte[] SplitTiff(byte[] original, int[] pages)
        {
            ImageCodecInfo _codecInfo = ImageCodecInfo.GetImageEncoders().First(k => k.MimeType == "image/tiff");
            EncoderParameters _encoderParameters = new EncoderParameters(1);

            Image _originalfile = Image.FromStream(new MemoryStream(original));
            int _pageCout = _originalfile.GetFrameCount(FrameDimension.Page);
            var _newFile = new MemoryStream();
            Image frame = null;

            if (_pageCout == 1)
                return original;

            var _p = pages.OrderBy(p => p).ToList();
            for (int i = 0; i < _pageCout; i++)
            {
                int tempNumner = i + 1;
                if (_p.Any(p => p == tempNumner))
                {
                    // set currunt page to frame
                    _originalfile.SelectActiveFrame(FrameDimension.Page, i);
                    // get currunt page as image
                    frame = _originalfile;

                    //add first frame
                    if(i == 0)
                    {
                        // Initialize the first frame of multipage tiff.                    
                        _encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.MultiFrame);
                        frame.Save(_newFile, _codecInfo, _encoderParameters);
                    }
                    else // add additional pages
                    {
                        // Add additional frames. 
                        _encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.FrameDimensionPage);
                        frame.SaveAdd(frame, _encoderParameters);
                    }
                }
            }

            if(_newFile.Length > 0)
            {
                // When it is the last frame, flush the resources and closing. 
                _encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.SaveFlag, (long)EncoderValue.Flush);
                frame.SaveAdd(_encoderParameters);
            }
            return _newFile.ToArray();
        }

    }
}
