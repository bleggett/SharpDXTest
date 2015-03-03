using SharpDX.Direct2D1;
using SharpDX.DXGI;
using SharpDX.WIC;
using System;
using System.IO;
using SharpDX.DirectWrite;

public class ImageCreator: IDisposable
{
    private readonly Direct2DFactoryManager _factoryManager;
    private readonly SharpDX.WIC.Bitmap _wicBitmap;
    private readonly WicRenderTarget _renderTarget;
    private readonly RenderTargetProperties _renderProperties;

    private readonly int _imageWidth, _imageHeight, _imageDpi;

    public ImageCreator(int imageWidth, int imageHeight, int imageDpi)
    {
        //If enabled, tracks undisposed COM objects
        //SharpDX.Configuration.EnableObjectTracking = true;
        
        this._imageWidth = imageWidth;
        this._imageHeight = imageHeight;
        this._imageDpi = imageDpi;

        _factoryManager = new Direct2DFactoryManager();

        _wicBitmap = new SharpDX.WIC.Bitmap(
            _factoryManager.WicFactory,
            imageWidth,
            imageHeight,
            SharpDX.WIC.PixelFormat.Format32bppBGR,
            BitmapCreateCacheOption.CacheOnDemand
            );

        _renderProperties = new RenderTargetProperties(
            RenderTargetType.Software,
            new SharpDX.Direct2D1.PixelFormat(Format.Unknown, AlphaMode.Unknown),
            imageDpi,
            imageDpi,
            RenderTargetUsage.None,
            FeatureLevel.Level_DEFAULT);

        _renderTarget = new WicRenderTarget(_factoryManager.D2DFactory, _wicBitmap, _renderProperties);
    }

    private void RenderImage()
    {
        using (SolidColorBrush blackBrush = new SolidColorBrush(_renderTarget, SharpDX.Color4.Black))
        using (TextFormat tformat = new TextFormat(_factoryManager.DwFactory, "Arial", 30f))
        using (TextFormat tformat2 = new TextFormat(_factoryManager.DwFactory, "Arial", 11f))
        {

            _renderTarget.BeginDraw();
            _renderTarget.Clear(SharpDX.Color.White);
            _renderTarget.DrawText("TEST", tformat, new SharpDX.RectangleF(300f, 30f, 100f, 20f), blackBrush);
            _renderTarget.DrawText("MORE TEST", tformat2, new SharpDX.RectangleF(30f, 150f, 100f, 20f), blackBrush);
            _renderTarget.DrawLine(new SharpDX.Vector2(0f, 25f), new SharpDX.Vector2(500f, 25f), blackBrush);
            _renderTarget.DrawLine(new SharpDX.Vector2(0f, 210f), new SharpDX.Vector2(500f, 210f), blackBrush);
            _renderTarget.EndDraw();
        }
    }

    public void SaveRenderedImage(Stream systemStream, Direct2DImageFormatType format)
    {
        RenderImage();

        using (WICStream wicStream = new WICStream(_factoryManager.WicFactory, systemStream))
        using (BitmapEncoder encoder = new BitmapEncoder(_factoryManager.WicFactory, Direct2DFormatMapper.ConvertImageFormat(format)))
        {
            encoder.Initialize(wicStream);

            using (BitmapFrameEncode bitmapFrameEncode = new BitmapFrameEncode(encoder))
            {

                bitmapFrameEncode.Initialize();
                bitmapFrameEncode.SetSize(_imageWidth, _imageHeight);
                bitmapFrameEncode.SetResolution(_imageDpi, _imageDpi);
                //bitmapFrameEncode.SetPixelFormat(new SharpDX.Direct2D1.PixelFormat(Format.R32G32B32A32_UInt, AlphaMode.Straight));
                bitmapFrameEncode.WriteSource(_wicBitmap);

                bitmapFrameEncode.Commit();
            }
            encoder.Commit();
        }
    }

    public void Dispose()
    {
        _wicBitmap.Dispose();
        _factoryManager.Dispose();
        _renderTarget.Dispose();
    }
}
