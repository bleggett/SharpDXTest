using System;


public class Direct2DFactoryManager : IDisposable
{
    private SharpDX.WIC.ImagingFactory wicFactory;
    private SharpDX.Direct2D1.Factory d2DFactory;
    private SharpDX.DirectWrite.Factory dwFactory;

    public Direct2DFactoryManager()
    {
        wicFactory = new SharpDX.WIC.ImagingFactory();
        d2DFactory = new SharpDX.Direct2D1.Factory();
        dwFactory = new SharpDX.DirectWrite.Factory();
    }

    public SharpDX.WIC.ImagingFactory WicFactory
    {
        get
        {
            return wicFactory;
        }
    }

    public SharpDX.Direct2D1.Factory D2DFactory
    {
        get
        {
            return d2DFactory;
        }
    }

    public SharpDX.DirectWrite.Factory DwFactory
    {
        get
        {
            return dwFactory;
        }
    }

    public void Dispose()
    {
        wicFactory.Dispose();
        d2DFactory.Dispose();
        dwFactory.Dispose();
    }
}
