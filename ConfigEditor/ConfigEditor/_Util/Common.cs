#region Usings
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Net.NetworkInformation;
using System.Linq;
using System.Windows.Media.Imaging;
using APS.WPF.SUB;
using Kornic.BlockControlFoundation.Drivers.Plc;
#endregion

namespace Kornic.BlockControlFoundation
{
  [StructLayout(LayoutKind.Sequential)]
  public struct SYSTEMTIME
  {
    public ushort wYear;
    public ushort wMonth;
    public ushort wDayOfWeek;
    public ushort wDay;
    public ushort wHour;
    public ushort wMinute;
    public ushort wSecond;
    public ushort wMilliseconds;
  }

  [Serializable]
  public class Common
  {
    #region class members
    /// <summary>
    /// 
    /// </summary>
    public static string DEF_INVALID_CHARS = "!@#$%^&*()+={}[]|<>?~`';:";
    /// <summary>
    /// 
    /// </summary>
    public static string DEF_COMBO_ITEM_ALL = "(ALL)";
    /// <summary>
    /// 
    /// </summary>
    public static int DEF_ONE_PAGE_MAX_ROWS = 500;
    /// <summary>
    /// 
    /// </summary>
    public static int DEF_CIM_T3_LIMIT_TIME = 20000;
    /// <summary>
    /// 
    /// </summary>
    public static int DEF_CIM_T3_DEFAULT_TIME = 500;
    /// <summary>
    /// 
    /// </summary>
    public const int DEF_EQ_LOCAL_LIMIT = 100;
    /// <summary>
    /// 
    /// </summary>
    public const int DEF_HISTORY_MAX_DAYS = 7;
    /// <summary>
    /// 
    /// </summary>
    public const int DEF_MAIN_UI_FDC_SCAN_TIME = 1500;
    #endregion

    #region class UI methods
    /// <summary>
    /// 
    /// </summary>
    public static BitmapImage GetImage(string sImageSource)
    {
      try
      {
        Assembly assembly = Assembly.GetExecutingAssembly();
        //string[] aaa = assembly.GetManifestResourceNames();
        
        BitmapImage image = new BitmapImage();

        using (Stream imageStream = assembly.GetManifestResourceStream("Kornic.BlockControlFoundation.Resource.Images." + sImageSource))
        {          
          image.BeginInit();
          image.StreamSource = imageStream;
          image.CacheOption = BitmapCacheOption.OnLoad;
          image.EndInit();
          image.Freeze();
          //imageStream.Dispose();
        }

        return image;
      }
      catch
      {
        return null;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public static long ListMaxPage(long iCount)
    {
      if (iCount > 0)
      {
        long lMax = (iCount / DEF_ONE_PAGE_MAX_ROWS) + (iCount % DEF_ONE_PAGE_MAX_ROWS != 0 ? 1 : 0);

        return (lMax == 0) ? 1 : lMax;
      }
      else
        return 1;
    }
    /// <summary>
    /// 
    /// </summary>
    public static object[] GetMaxPageAry(long iMaxPage)
    {
      var list = new List<object>();
      for (int i = 1; i <= iMaxPage; i++)
      {
        list.Add(i);
      }

      return list.ToArray();
    }
    /// <summary>
    /// 
    /// </summary>
    public static long[] InfoPageArray(long iMaxPage)
    {
      long[] aryPage = new long[iMaxPage];
      for (int i = 1; i <= iMaxPage; i++)
      {
        aryPage[i - 1] = i;
      }

      return aryPage;
    }
    /// <summary>
    /// 
    /// </summary>
    public static void DynamicUsing(object resource, Action action)
    {
      try
      {
        action();
      }
      finally
      {
        IDisposable d = resource as IDisposable;
        if (d != null)
          d.Dispose();
      }
    }
    #endregion

    #region class application methods
    /// <summary>
    /// 
    /// </summary>
    public static string GetApplicationName()
    {
      return string.Format("{0}", Assembly.GetExecutingAssembly().GetName().Name);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GetApplicationDescription()
    {
      return string.Format("{0}", Assembly.GetExecutingAssembly().GetName().FullName.Substring(Assembly.GetExecutingAssembly().GetName().Name.Length + 2));
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GetApplicationSoftVersion()
    {
      return string.Format("{0}", Assembly.GetExecutingAssembly().GetName().Version).Substring(0, 6).Trim('.');
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GetApplicationFileDate(bool bReport = true)
    {
      string sFileLocation = Assembly.GetExecutingAssembly().Location;

      FileInfo file = new FileInfo(sFileLocation);

      return string.Format("{0}", bReport ? GenerateReportTime(file.LastWriteTime) : GenerateCurrentTime(file.LastWriteTime));
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GetApplicationFileSize()
    {
      string sFileLocation = Assembly.GetExecutingAssembly().Location;

      FileInfo file = new FileInfo(sFileLocation);

      return string.Format("{0} Kbyte", file.Length / 1024);
    }
    #endregion

    #region class Times methods
    /// <summary>
    /// 
    /// </summary>
    public static string GenerateCurrentTime()
    {
      return string.Format("{0:yy}.{0:MM}.{0:dd} {0:HH}:{0:mm}:{0:ss}.{0:fff}", DateTime.Now);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GenerateCurrentTimeSS()
    {
      return string.Format("{0:yyyy}{0:MM}{0:dd}{0:HH}{0:mm}{0:ss}.{0:fff}", DateTime.Now);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GenerateCurrentTime(DateTime time)
    {
      return string.Format("{0:yy}.{0:MM}.{0:dd} {0:HH}:{0:mm}:{0:ss}.{0:fff}", time);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GenerateShortCurrentTime()
    {
      return string.Format("{0:yy}.{0:MM}.{0:dd} {0:HH}:{0:mm}:{0:ss}", DateTime.Now);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GenerateReportTime(DateTime time)
    {
      return string.Format("{0:yyyy}{0:MM}{0:dd}{0:HH}{0:mm}{0:ss}", time);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GenerateReportTime()
    {
      return string.Format("{0:yyyy}{0:MM}{0:dd}{0:HH}{0:mm}{0:ss}", DateTime.Now);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GenerateRepoteLongTime(DateTime time)
    {
      return string.Format("{0:yyyy}-{0:MM}-{0:dd} {0:HH}:{0:mm}:{0:ss} 00", time);
    }
    /// <summary>
    /// 
    /// </summary>
    public static DateTime GenerateStringToTime(string sTime)
    {
      int year = int.Parse(sTime.Substring(0, 2)) + 2000;
      int month = int.Parse(sTime.Substring(3, 2));
      int day = int.Parse(sTime.Substring(6, 2));
      int hour = int.Parse(sTime.Substring(9, 2));
      int minute = int.Parse(sTime.Substring(12, 2));
      int second = int.Parse(sTime.Substring(15, 2));
      int mili = int.Parse(sTime.Substring(18, 3));

      return new DateTime(year, month, day, hour, minute, second, mili);
    }
    /// <summary>
    /// 
    /// </summary>
    public static DateTime GeneratePLCToTime(string sTime)
    {
      if (string.IsNullOrEmpty(sTime))
        return DateTime.Now;

      int year = int.Parse(sTime.Substring(0, 4));
      int month = int.Parse(sTime.Substring(4, 2));
      int day = int.Parse(sTime.Substring(6, 2));
      int hour = int.Parse(sTime.Substring(8, 2));
      int minute = int.Parse(sTime.Substring(10, 2));
      int second = int.Parse(sTime.Substring(12, 2));

      return new DateTime(year, month, day, hour, minute, second);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GenerateTimestamp()
    {
      return string.Format("{0:yyyy}{0:MM}{0:dd}{0:HH}{0:mm}{0:ss}", DateTime.Now);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GenerateTimestamp(DateTime time)
    {
      return string.Format("{0:yyyy}{0:MM}{0:dd}{0:HH}{0:mm}{0:ss}", time);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GenerateTimestampSS(DateTime time)
    {
      return string.Format("{0:yyyy}{0:MM}{0:dd}{0:HH}{0:mm}{0:ss}{0:ff}", time);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string GenerateGUITime(DateTime time)
    {
      string sCurrentTime = string.Format("{0:yyyy}-{0:MM}-{0:dd} {0:HH}:{0:mm}:{0:ss} {0:ff}", time);

      return sCurrentTime;
    }
    /// <summary>
    /// 
    /// </summary>
    public static DateTime GenerateGUITime(string sTime)
    {
      try
      {
        int year = Convert.ToInt32(sTime.Substring(0, 4));
        int month = Convert.ToInt32(sTime.Substring(5, 2));
        int day = Convert.ToInt32(sTime.Substring(8, 2));
        int hour = Convert.ToInt32(sTime.Substring(11, 2));
        int minute = Convert.ToInt32(sTime.Substring(14, 2));
        int second = Convert.ToInt32(sTime.Substring(17, 2));
        int milsecond = Convert.ToInt32(sTime.Substring(20, 2)) * 10;

        DateTime time = new DateTime(year, month, day, hour, minute, second, milsecond, DateTimeKind.Local);

        return time;
      }
      catch
      {
        return DateTime.Now;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public static DateTime GetFromTime()
    {
      return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 0, 0, 0);
    }
    /// <summary>
    /// 
    /// </summary>
    public static DateTime GetToTime()
    {
      return new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 59);
    }
    /// <summary>
    /// 
    /// </summary>
    public static DateTime GetFromTime(DateTime? date)
    {
      return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, date.Value.Hour, date.Value.Minute, date.Value.Second);
    }
    /// <summary>
    /// 
    /// </summary>
    public static DateTime GetToTime(DateTime? date)
    {
      return new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, date.Value.Hour, date.Value.Minute, date.Value.Second);
    }
    /// <summary>
    /// 
    /// </summary>
    public static DateTime GenerateBCDToTime(ushort[] AryBCD)
    {
      try
      {
        int iYear = HEX2DEC(GetBitData(AryBCD[0], 0, 7)) + 2000;
        int iMonth = HEX2DEC(GetBitData(AryBCD[0], 8, 15));
        int iDay = HEX2DEC(GetBitData(AryBCD[1], 0, 7));
        int iHour = HEX2DEC(GetBitData(AryBCD[1], 8, 15));
        int iMinute = HEX2DEC(GetBitData(AryBCD[2], 0, 7));
        int iSecond = HEX2DEC(GetBitData(AryBCD[2], 8, 15));

        return new DateTime(iYear, iMonth, iDay, iHour, iMinute, iSecond);
      }
      catch { return DateTime.Now; }
    }
    /// <summary>
    /// 
    /// </summary>
    public static ushort[] GenerateTimeToBCD(DateTime time)
    {
      ushort[] AryBCD = new ushort[3];

      AryBCD[0] = (ushort)((DEC2HEX(time.Month) << 8) + DEC2HEX(time.Year - 2000));
      AryBCD[1] = (ushort)((DEC2HEX(time.Hour) << 8) + DEC2HEX(time.Day));
      AryBCD[2] = (ushort)((DEC2HEX(time.Second) << 8) + DEC2HEX(time.Minute));

      return AryBCD;
    }
    #endregion

    #region class PLC methods
    /// <summary>
    /// 
    /// </summary>
    public static string TrimChar(string sText)
    {
      if (string.IsNullOrEmpty(sText))
        return string.Empty;

      return sText.TrimEnd().Trim('\0').Replace('\0', ' ');
    }
    /// <summary>
    /// 
    /// </summary>
    public static string PLCPointValue(object oPlcValue)
    {
      double dValue = Convert.ToDouble(oPlcValue) * 0.0001;

      string sValue = string.Format("{0:0.####}", dValue);

      return sValue;
    }
    /// <summary>
    /// 
    /// </summary>
    public static string PLCPointValue(object oPlcValue, double dPoint, bool bDotCut = true)
    {
      double dValue = Convert.ToDouble(oPlcValue) * dPoint;

      string sDot = bDotCut ? "{0:0.##}" : "{0:0.#########}";

      string sValue = string.Format(sDot, dValue);

      return sValue;
    }
    /// <summary>
    /// 
    /// </summary>
    public static string PLCPointEValue(object oPlcValue)
    {
      double dValue = Convert.ToDouble(oPlcValue);

      string sValue = string.Format("{0:E2}", dValue);

      return sValue;
    }
    /// <summary>
    /// 
    /// </summary>
    public static string ParseIntToBitString(int[] AryValue)
    {
      BitArray AryBit = new BitArray(AryValue);
      char[] bits = AryBit.Cast<bool>().Select(bit => bit ? '1' : '0').ToArray();
      Array.Reverse(bits);

      return new string(bits);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string StrTrim(string str)
    {
      str = str.Replace('\0', ' ');

      return str.Trim('\r').Trim('\n').Trim('\t').Trim();
    }
    /// <summary>
    /// 
    /// </summary>
    public static Int32 DEC2HEX(Int32 iValue)
    {
      try
      {
        return int.Parse(iValue.ToString(), System.Globalization.NumberStyles.HexNumber);
      }
      catch
      {
        return -1;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public static Int32 HEX2DEC(Int32 iValue)
    {
      try
      {
        return int.Parse(string.Format("{0:X}", iValue));
      }
      catch
      {
        return -1;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public static ushort GetBitData(ushort iValue, int iStart, int iEnd)
    {
      iValue = (ushort)(iValue << (15 - iEnd));

      iValue = (ushort)(iValue >> (15 - (iEnd - iStart)));

      return iValue;
    }
    /// <summary>
    /// 
    /// </summary>
    public static uint GetBitData(uint iValue, int iStart, int iEnd)
    {
      iValue = (uint)(iValue << (31 - iEnd));

      iValue = (uint)(iValue >> (31 - (iEnd - iStart)));

      return iValue;
    }
    /// <summary>
    /// 
    /// </summary>
    public static bool ReadBit(ushort iWord, Int32 iIndex)
    {
      return (1 == (1 & (iWord >> iIndex)));
    }
    /// <summary>
    /// 
    /// </summary>
    public static byte[] SliceIntegerIntoBytes(int iValue, int iBytesNo)
    {
      byte[] arr = new byte[iBytesNo];

      for (int i = 0; i < iBytesNo; ++i)
      {
        int iNextByte = iValue & 255;
        arr[i] = (byte)iNextByte;

        iValue >>= 8;
      }

      return arr;
    }
    /// <summary>
    /// 
    /// </summary>
    public static UInt32 SliceUInt16IntoUInt32(UInt16 iFirst, UInt16 iSecond)
    {
      UInt32 iValue = 0;

      iValue = (UInt32)iSecond << 16;
      iValue += iFirst;

      return iValue;
    }
    /// <summary>
    /// 
    /// </summary>
    public static string ArrayToCscSplit(string[] aryValue)
    {
      StringBuilder sb = new StringBuilder();

      foreach (string data in aryValue)
      {
        sb.Append(string.Format("{0},", data));
      }

      return sb.ToString().TrimEnd(',');
    }
    /// <summary>
    /// 
    /// </summary>
    public static string[] WordStructToArray(PlcWordStruct plcWordStruct)
    {
      if (plcWordStruct != null)
      {
        string[] aryValue = new string[plcWordStruct.Count];

        for (int i = 0; i < plcWordStruct.Count; i++)
        {
          aryValue[i] = plcWordStruct[i].GetString();
        }

        return aryValue;
      }

      return null;
    }
    #endregion

    #region class ReadAttribute methods
    /// <summary>
    /// 
    /// </summary>
    public static bool ShouldBreakXmlReading(XmlReader reader, string sOpenNodeName)
    {
      bool bShouldBreakXmlReading =
          (XmlNodeType.EndElement == reader.NodeType) &&
          (0 == string.Compare(reader.Name, sOpenNodeName, true));

      return bShouldBreakXmlReading;
    }
    /// <summary>
    /// 
    /// </summary>
    public static bool ShouldSkipXmlNode(XmlReader reader)
    {
      bool bShouldSkip =
              (string.IsNullOrEmpty(reader.Name)) ||
              (XmlNodeType.Element != reader.NodeType) ||
              (XmlNodeType.EndElement == reader.NodeType);

      return bShouldSkip;
    }
    /// <summary>
    /// 
    /// </summary>
    public static string ReadAttribute(XmlNode node, string sName)
    {
      return ReadAttribute(node, sName, string.Empty);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string ReadAttribute(XmlReader reader, string sName)
    {
      return ReadAttribute(reader, sName, string.Empty);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string ReadAttribute(XmlNode node, string sName, string sDefaultValue)
    {
      string str = sDefaultValue;
      try
      {
        return node.Attributes[sName].Value;
      }
      catch (Exception)
      {
        return sDefaultValue;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public static string ReadAttribute(XmlReader reader, string sName, string sDefaultValue)
    {
      if (!reader.HasAttributes)
      {
        return sDefaultValue;
      }
      string attribute = sDefaultValue;
      try
      {
        attribute = reader.GetAttribute(sName);
      }
      catch (Exception)
      {
        attribute = sDefaultValue;
      }
      return ((attribute == null) ? sDefaultValue : attribute);
    }
    /// <summary>
    /// 
    /// </summary>
    public static bool ReadAttributeAsBool(XmlNode node, string sName)
    {
      return ReadAttributeAsBool(node, sName, false);
    }
    /// <summary>
    /// 
    /// </summary>
    public static bool ReadAttributeAsBool(XmlNode node, string sName, bool bDefaultValue)
    {
      string sValue = ReadAttribute(node, sName);
      if (string.IsNullOrEmpty(sValue))
      {
        return bDefaultValue;
      }
      bool flag = bDefaultValue;
      try
      {
        flag = 0 == string.Compare(sValue, bool.TrueString, true);
      }
      catch (Exception)
      {

      }
      return flag;
    }
    /// <summary>
    /// 
    /// </summary>
    public static bool ReadAttributeAsBool(XmlReader reader, string sName, bool bDefaultValue)
    {
      string sValue = ReadAttribute(reader, sName);
      if (string.IsNullOrEmpty(sValue))
      {
        return bDefaultValue;
      }
      bool flag = bDefaultValue;
      try
      {
        flag = Convert.ToBoolean(sValue);
      }
      catch (Exception)
      {
      }
      return flag;
    }
    /// <summary>
    /// 
    /// </summary>
    public static double ReadAttributeAsDouble(XmlReader reader, string sName, double iDefaultValue)
    {
      string str = ReadAttribute(reader, sName);
      if (string.IsNullOrEmpty(str))
      {
        return iDefaultValue;
      }
      double num = iDefaultValue;
      try
      {
        num = Convert.ToDouble(str);
      }
      catch (Exception)
      {
      }
      return num;
    }
    /// <summary>
    /// 
    /// </summary>
    public static Int32 ReadAttributeAsEnum(XmlNode node, string sName, Type enumType, Int32 iDefaultValue)
    {
      string sValue = ReadAttribute(node, sName);
      if (string.IsNullOrEmpty(sValue))
      {
        return iDefaultValue;
      }
      Int32 num = iDefaultValue;
      try
      {
        num = (int)Enum.Parse(enumType, sValue, true);
      }
      catch (Exception)
      {
      }
      return num;
    }
    /// <summary>
    /// 
    /// </summary>
    public static Int32 ReadAttributeAsEnum(XmlReader reader, string sName, Type enumType, Int32 iDefaultValue)
    {
      string sValue = ReadAttribute(reader, sName);
      if (string.IsNullOrEmpty(sValue))
      {
        return iDefaultValue;
      }
      Int32 num = iDefaultValue;
      try
      {
        num = (int)Enum.Parse(enumType, sValue, true);
      }
      catch (Exception)
      {
      }
      return num;
    }
    /// <summary>
    /// 
    /// </summary>
    public static Int32 ReadAttributeAsInt(XmlNode node, string sName)
    {
      return ReadAttributeAsInt(node, sName, 0);
    }
    /// <summary>
    /// 
    /// </summary>
    public static Int32 ReadAttributeAsInt(XmlReader reader, string sName)
    {
      return ReadAttributeAsInt(reader, sName, 0);
    }
    /// <summary>
    /// 
    /// </summary>
    public static Int32 ReadAttributeAsInt(XmlNode node, string sName, Int32 iDefaultValue)
    {
      string sValue = ReadAttribute(node, sName);
      if (string.IsNullOrEmpty(sValue))
      {
        return iDefaultValue;
      }
      Int32 num = iDefaultValue;
      try
      {
        num = Convert.ToInt32(sValue);
      }
      catch (Exception)
      {
      }
      return num;
    }
    /// <summary>
    /// 
    /// </summary>
    public static Int32 ReadAttributeAsInt(XmlReader reader, string sName, Int32 iDefaultValue)
    {
      string sValue = ReadAttribute(reader, sName);
      if (string.IsNullOrEmpty(sValue))
      {
        return iDefaultValue;
      }
      Int32 num = iDefaultValue;
      try
      {
        num = Convert.ToInt32(sValue);
      }
      catch (Exception)
      {
      }
      return num;
    }
    /// <summary>
    /// 
    /// </summary>
    public static UInt32 ReadAttributeAsUInt32(XmlNode node, string sName)
    {
      return ReadAttributeAsUInt32(node, sName, 0);
    }
    /// <summary>
    /// 
    /// </summary>
    public static UInt32 ReadAttributeAsUInt32(XmlReader reader, string sName)
    {
      return ReadAttributeAsUInt32(reader, sName, 0);
    }
    /// <summary>
    /// 
    /// </summary>
    public static UInt32 ReadAttributeAsUInt32(XmlNode node, string sName, UInt32 iDefaultValue)
    {
      string sValue = ReadAttribute(node, sName);
      if (string.IsNullOrEmpty(sValue))
      {
        return iDefaultValue;
      }
      UInt32 num = iDefaultValue;
      try
      {
        num = uint.Parse(sValue);
      }
      catch (Exception)
      {
      }
      return num;
    }
    /// <summary>
    /// 
    /// </summary>
    public static UInt32 ReadAttributeAsUInt32(XmlReader reader, string sName, UInt32 iDefaultValue)
    {
      string sValue = ReadAttribute(reader, sName);
      if (string.IsNullOrEmpty(sValue))
      {
        return iDefaultValue;
      }
      UInt32 num = iDefaultValue;
      try
      {
        num = Convert.ToUInt32(sValue);
      }
      catch (Exception)
      {
      }
      return num;
    }
    /// <summary>
    /// 
    /// </summary>
    public static string ReadElementValue(XmlReader reader)
    {
      return ReadElementValue(reader, string.Empty);
    }
    /// <summary>
    /// 
    /// </summary>
    public static string ReadElementValue(XmlReader reader, string sDefaultValue)
    {
      return (reader.IsEmptyElement ? sDefaultValue : reader.ReadElementContentAsString());
    }
    #endregion

    #region class LineControl methods
    public static int GetLineControlBufferOffset(int iActionPath)
    {
      int iValue = 75; // 기본 offset 값 추가. RECV_WAIT = 275.

      switch (iActionPath) // Station 번호에 따른 offset
      {
        case 0:
        case 1:
          iValue += iActionPath;
          break;

        case 2:
          iValue += (9 + iActionPath);
          break;

        case 3:
          iValue += (iActionPath - 1);
          break;

        case 4:
          iValue += (6 + iActionPath);
          break;
      }

      return iValue;
    }
    #endregion

    #region class etc methods
    /// <summary>
    /// 
    /// </summary>
    public static bool CompareJudgeIncludeNG(string sJudgeCode)
    {
      switch(sJudgeCode)
      {
        case "N":
        case "D":
        case "PE":
          return true;

        default:
          return false;
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public static bool CompareMixRunFlag(int EvRoute)
    {
      return (EvRoute == 5 || EvRoute == 6);  // 5 = MIX_A, 6 = MIX_B
    }
    /// <summary>
    /// 
    /// </summary>
    public static ConnectStatusBar ReturnStatusAttribute(UIElement control, string sLabelName)
    {
      if (string.IsNullOrEmpty(sLabelName))
        return null;

      FieldInfo info = control.GetType().GetField(sLabelName, BindingFlags.GetField | BindingFlags.IgnoreCase
                                                      | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
      return info.GetValue(control) as ConnectStatusBar;
    }
    /// <summary>
    /// 
    /// </summary>
    public static string CheckBoxContent(string sContent)
    {
      int iIndex = sContent.IndexOf('_');

      if (iIndex < 0)
      {
        return sContent;
      }
      else
      {
        return sContent.Insert(iIndex, "_");
      }
    }
    /// <summary>
    /// 
    /// </summary>
    public static double GetExcelRound(double value, int digit)
    {
      var val1 = double.Parse((0.01 * Math.Pow(0.1, digit - 1)).ToString(), System.Globalization.NumberStyles.Float);
      var val2 = 100 * Math.Pow(10, digit - 1);

      return Math.Round(value * val1, 1, MidpointRounding.AwayFromZero) * val2;
    }
    /// <summary>
    /// 
    /// </summary>
    public static double GetExcelCeiling(double value, int digit)
    {
      var val1 = double.Parse((0.01 * Math.Pow(0.1, digit - 1)).ToString(), System.Globalization.NumberStyles.Float);
      var val2 = 100 * Math.Pow(10, digit - 1);

      return Math.Ceiling(value * val1) * val2;
    }
    /// <summary>
    /// 
    /// </summary>
    public static double GetExcelFloor(double value, int digit)
    {
      var val1 = double.Parse((0.01 * Math.Pow(0.1, digit - 1)).ToString(), System.Globalization.NumberStyles.Float);
      var val2 = 100 * Math.Pow(10, digit - 1);

      return Math.Floor(value * val1) * val2;
    }
    #endregion
  }
}
