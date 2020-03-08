using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using static Z_Toolbar.MainLogic.NativeMethods;

namespace Z_Toolbar.MainLogic
{
    public static class UITools
    {
        private const uint FILE_ATTRIBUTE_NORMAL = 0x00000080;
        private const uint FILE_ATTRIBUTE_DIRECTORY = 0x00000010;

        /// <summary>
        /// Converts a <see cref="System.Drawing.Bitmap"/> into a WPF <see cref="BitmapSource"/>.
        /// </summary>
        /// <remarks>Uses GDI to do the conversion. Hence the call to the marshalled DeleteObject.
        /// </remarks>
        /// <param name="source">The source bitmap.</param>
        /// <returns>A BitmapSource</returns>
        public static BitmapSource ToBitmapSource(this System.Drawing.Bitmap source)
        {
            BitmapSource bitSrc = null;

            var hBitmap = source.GetHbitmap();

            try
            {
                bitSrc = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Win32Exception)
            {
                bitSrc = null;
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return bitSrc;
        }

        public static ObservableCollection<ApplicationItem> PopulateIcons(ObservableCollection<ApplicationItem> loa)
        {
            foreach (var item in loa)
            {
                if (item.IsFolder)
                {
                    item.Icon = (BitmapSource)GetIcon(item.Path, true);
                }
                else
                {
                    item.Icon = (BitmapSource)GetIcon(item.Path, false);
                }
            }

            return loa;
        }

        public static BitmapSource GetIcon(string path,bool isDirectory)
        {
            IntPtr hIcon = GetJumboIcon(GetIconIndex(path,isDirectory));
            BitmapSource icon = null;

            using (Icon ico = (Icon)System.Drawing.Icon.FromHandle(hIcon).Clone())
            {

                icon = ico.ToBitmap().ToBitmapSource();
            }

            DestroyIcon(hIcon);

            return icon;
        }

        internal static int GetIconIndex(string pszFile,bool isDirectory)
        {
            uint attributes = FILE_ATTRIBUTE_NORMAL;
            if (isDirectory)
                attributes |= FILE_ATTRIBUTE_DIRECTORY;

            SHFILEINFO sfi = new SHFILEINFO();
            NativeMethods.SHGetFileInfo(pszFile
                , attributes //0
                , ref sfi
                , (uint)System.Runtime.InteropServices.Marshal.SizeOf(sfi)
                , (uint)(SHGFI.SysIconIndex | SHGFI.LargeIcon | SHGFI.UseFileAttributes));
            return sfi.iIcon;
        }

        // 256*256
        internal static IntPtr GetJumboIcon(int iImage)
        {
            IImageList spiml = null;
            Guid guil = new Guid(IID_IImageList); //or IID_IImageList2

            SHGetImageList(SHIL_EXTRALARGE, ref guil, ref spiml);
            IntPtr hIcon = IntPtr.Zero;
            spiml.GetIcon(iImage, ILD_TRANSPARENT | ILD_IMAGE, ref hIcon); //

            return hIcon;
        }

    }
}
