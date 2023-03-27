using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
public class ImageStatistics
{
    private byte[] _pixels;
    private int _width;
    private int _height;

    public ImageStatistics(string filePath)
    {
        LoadImage(filePath);
    }

    private void LoadImage(string filePath)
    {
        using (var image = new Bitmap(filePath))
        {
            _width = image.Width;
            _height = image.Height;
            var data = image.LockBits(new Rectangle(0, 0, _width, _height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);

            _pixels = new byte[_width * _height];
            Marshal.Copy(data.Scan0, _pixels, 0, _pixels.Length);

            image.UnlockBits(data);
        }
    }

    public double GetMean()
    {
        double sum = 0;

        for (int i = 0; i < _pixels.Length; i++)
        {
            sum += _pixels[i];
        }

        return sum / _pixels.Length;
    }

    public double GetStandardDeviation()
    {
        double mean = GetMean();
        double sumSquaredDiff = 0;

        for (int i = 0; i < _pixels.Length; i++)
        {
            double diff = _pixels[i] - mean;
            sumSquaredDiff += diff * diff;
        }

        double variance = sumSquaredDiff / _pixels.Length;
        return Math.Sqrt(variance);
    }

    public byte GetMinGrayValue()
    {
        byte min = byte.MaxValue;

        for (int i = 0; i < _pixels.Length; i++)
        {
            if (_pixels[i] < min)
            {
                min = _pixels[i];
            }
        }

        return min;
    }

    public byte GetMaxGrayValue()
    {
        byte max = byte.MinValue;

        for (int i = 0; i < _pixels.Length; i++)
        {
            if (_pixels[i] > max)
            {
                max = _pixels[i];
            }
        }

        return max;
    }
}


public class Program
{
    public static void Main(string[] args)
    {
        string filePath = "G:/Progress soft/Q4 Progress soft/Test.jpg";
        var stats = new ImageStatistics(filePath);

        double mean = stats.GetMean();
        double standardDeviation = stats.GetStandardDeviation();
        byte minGrayValue = stats.GetMinGrayValue();
        byte maxGrayValue = stats.GetMaxGrayValue();

        Console.WriteLine($"Mean: {mean}");
        Console.WriteLine($"Standard deviation: {standardDeviation}");
        Console.WriteLine($"Minimum gray value: {minGrayValue}");
        Console.WriteLine($"Maximum gray value: {maxGrayValue}");
    }
}