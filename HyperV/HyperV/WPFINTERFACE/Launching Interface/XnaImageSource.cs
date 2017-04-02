using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Runtime.InteropServices;
using System.Windows;

namespace Launching_Interface
{
    class XnaImageSource : IDisposable
    {
        // the render target we draw to
        private RenderTarget2D renderTarget;

        // a WriteableBitmap we copy the pixels into for 
        // display into the Image
        private WriteableBitmap writeableBitmap;

        // a buffer array that gets the data from the render target
        private byte[] buffer;

        /// <summary>
        /// Gets the render target used for this image source.
        /// </summary>
        public RenderTarget2D RenderTarget
        {
            get { return renderTarget; }
        }

        /// <summary>
        /// Gets the underlying WriteableBitmap that can 
        /// be bound as an ImageSource.
        /// </summary>
        public WriteableBitmap WriteableBitmap
        {
            get { return writeableBitmap; }
        }

        /// <summary>
        /// Creates a new XnaImageSource.
        /// </summary>
        /// <param name="graphics">The GraphicsDevice to use.</param>
        /// <param name="width">The width of the image source.</param>
        /// <param name="height">The height of the image source.</param>
        public XnaImageSource(GraphicsDevice graphics, int width, int height)
        {
            // create the render target and buffer to hold the data
            renderTarget = new RenderTarget2D(
                graphics, width, height, false,
                SurfaceFormat.Color,
                DepthFormat.Depth24Stencil8);
            buffer = new byte[width * height * 4];
            writeableBitmap = new WriteableBitmap(
                width, height, 96, 96,
                PixelFormats.Bgra32, null);
        }

        ~XnaImageSource()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            renderTarget.Dispose();

            if (disposing)
                GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Commits the render target data into our underlying bitmap source.
        /// </summary>
        public void Commit()
        {
            // get the data from the render target
            renderTarget.GetData(buffer);

            // because the only 32 bit pixel format for WPF is 
            // BGRA but XNA is all RGBA, we have to swap the R 
            // and B bytes for each pixel
            for (int i = 0; i < buffer.Length - 2; i += 4)
            {
                byte r = buffer[i];
                buffer[i] = buffer[i + 2];
                buffer[i + 2] = r;
            }

            // write our pixels into the bitmap source
            writeableBitmap.Lock();
            Marshal.Copy(buffer, 0, writeableBitmap.BackBuffer, buffer.Length);
            writeableBitmap.AddDirtyRect(
                new Int32Rect(0, 0, renderTarget.Width, renderTarget.Height));
            writeableBitmap.Unlock();
        }
    }
}
