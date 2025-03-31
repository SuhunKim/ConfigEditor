using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.IO;
using System.Text;

namespace Kornic.BlockControlFoundation
{
  public interface PrivateINIBasic
  {
    string[] GetAllSectionNames();

    string[] GetAllSectionValue(string SectionName);

    string GetValue(string SectionName, string KeyName);

    bool SetValue(string SectionName, string KeyName, string Value);

    void DeleteSection(string SectionName);

    bool DeleteKey(string SectionName, string KeyName);

    bool SetPairBySection(string SectionName, string KeyName, string Value);

    string[] GetPairsBySection(string SectionName);
  }

  internal static class INIAPI
  {
    [DllImport("kernel32.dll")]
    public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

    [DllImport("kernel32.dll")]
    public static extern bool WritePrivateProfileSection(string lpAppName, string lpString, string lpFileName);

    [DllImport("kernel32.dll")]
    public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefalut, string lpFileName);

    [DllImport("kernel32.dll")]
    public static extern uint GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, byte[] lpReturnValue, uint nSize, string lpFileName);

    [DllImport("kernel32.dll")]
    public static extern uint GetPrivateProfileSection(string lpAppName, byte[] lpPairVaules, uint nSize, string lpFileName);

    [DllImport("kernel32.dll")]
    public static extern uint GetPrivateProfileSectionNames(byte[] lpSections, uint nSize, string lpFileName);
  }

  class INI : PrivateINIBasic
  {
    #region Class statics
    /// <summary>
    /// 
    /// </summary>
    public static uint SectionBufferSize = 1024;
    /// <summary>
    /// 
    /// </summary>
    public static uint DataBufferSize = 65534;
    /// <summary>
    /// 
    /// </summary>
    protected static string m_sFileName;
    #endregion

    #region Class initialization
    /// <summary>
    /// 
    /// </summary>
    /// <param name="m_FileTotalName"></param>
    public INI(string FileTotalName)
    {
      m_sFileName = FileTotalName;

      FileInfo fi = new FileInfo(m_sFileName);
      if (!fi.Directory.Exists)
      {
        fi.Directory.Create();
      }
    }
    #endregion

    #region Class public methods
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string[] GetAllSectionNames()
    {
      if (!FileExist()) throw (new FileNotFoundException());

      byte[] bSection = new byte[SectionBufferSize];

      if (INIAPI.GetPrivateProfileSectionNames(bSection, SectionBufferSize, m_sFileName) <= 0)
      {
        return null;
      }

      return System.Text.Encoding.Default.GetString(bSection).Split(new char[1] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="SectionName"></param>
    /// <returns></returns>
    public string[] GetAllSectionValue(string SectionName)
    {
      if (!FileExist()) throw (new FileNotFoundException());

      byte[] bData = new byte[DataBufferSize];

      if (INIAPI.GetPrivateProfileSection(SectionName, bData, DataBufferSize, m_sFileName) <= 0)
      {
        return null;
      }

      return System.Text.Encoding.Default.GetString(bData).Split(new char[1] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="SectionName"></param>
    /// <param name="KeyName"></param>
    /// <returns></returns>
    public string GetValue(string SectionName, string KeyName)
    {
      if (!FileExist()) throw (new FileNotFoundException());

      byte[] bPair = new byte[DataBufferSize];
      if (INIAPI.GetPrivateProfileString(SectionName, KeyName, "", bPair, DataBufferSize, m_sFileName) <= 0)
      {
        return string.Empty;
      }

      return System.Text.Encoding.Default.GetString(bPair).Trim('\0');
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="SectionName"></param>
    /// <param name="KeyName"></param>
    /// <param name="iDefault"></param>
    /// <returns></returns>
    public int GetValueToInt(string SectionName, string KeyName, int iDefault)
    {
      string sValue = GetValue(SectionName, KeyName);

      try
      {
        int iValue = iDefault;
        int.TryParse(sValue, out iValue);

        return iValue;
      }
      catch { return iDefault; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="SectionName"></param>
    /// <param name="KeyName"></param>
    /// <param name="dDefault"></param>
    /// <returns></returns>
    public double GetValueToDouble(string SectionName, string KeyName, double dDefault)
    {
      string sValue = GetValue(SectionName, KeyName);

      try
      {
        double dValue = dDefault;
        double.TryParse(sValue, out dValue);

        return dValue;
      }
      catch { return dDefault; }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="SectionName"></param>
    /// <param name="KeyName"></param>
    /// <param name="Value"></param>
    /// <returns></returns>
    public bool SetValue(string SectionName, string KeyName, string Value)
    {
      if (!FileExist()) throw (new FileNotFoundException());

      return INIAPI.WritePrivateProfileString(SectionName, KeyName, Value, m_sFileName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="SectionName"></param>
    /// <returns></returns>
    public string[] GetPairsBySection(string SectionName)
    {
      if (!FileExist()) throw (new FileNotFoundException());
      byte[] bPair = new byte[DataBufferSize];
      if (INIAPI.GetPrivateProfileSection(SectionName, bPair, DataBufferSize, m_sFileName) <= 0)
      {
        return null;
      }
      return System.Text.Encoding.Default.GetString(bPair).Split(new char[1] { '\0' }, StringSplitOptions.RemoveEmptyEntries);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="szSection"></param>
    /// <param name="szKey"></param>
    /// <param name="szValue"></param>
    /// <returns></returns>
    public bool SetPairBySection(string szSection, string szKey, string szValue)
    {
      return INIAPI.WritePrivateProfileString(szSection, szKey, szValue, m_sFileName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="szSection"></param>
    /// <param name="szKey"></param>
    /// <returns></returns>
    public bool DeleteKey(string szSection, string szKey)
    {
      return INIAPI.WritePrivateProfileString(szSection, szKey, null, m_sFileName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="szSection"></param>
    public void DeleteSection(string szSection)
    {
      INIAPI.WritePrivateProfileSection(szSection, null, m_sFileName);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private bool FileExist()
    {
      if (System.IO.File.Exists(m_sFileName))
      {
        return true;
      }

      return false;
    }

    #endregion
  }
}
