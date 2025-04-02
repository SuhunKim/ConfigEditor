using ConfigEditor;
using JMKIM.XmlControl;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml;

namespace Kornic.BlockControlFoundation
{
	public class Configurator
	{
		#region Class constants
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_TOP_ELEMENT = "CIM";
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_ATTR_PROJECT = "Project";
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_ATTR_LINE = "Line";
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_ATTR_DB = "DB";
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_ATTR_APPLICATION_NAME = "ApplicationName";
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_ATTR_PASSWORD = "Password";
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_ATTR_LOGIN = "login";
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_ATTR_PATH = "path";
		#endregion

		#region Class members
		/// <summary>
		/// 
		/// </summary>
		private MainWindow m_mainWindow;
		/// <summary>
		/// 
		/// </summary>
		private PLCManager m_plcManager;
		/// <summary>
		/// 
		/// </summary>
		private SvidManager m_svidManager;
		/// <summary>
		/// 
		/// </summary>
		private string m_sConfigPath;
		/// <summary>
		/// 
		/// </summary>
		private XmlDataReader m_xmlReader;
		/// <summary>
		/// 
		/// </summary>
		private string m_sUtilityPath;
		/// <summary>
		/// 
		/// </summary>
		private string m_sDataStructPath;
		/// <summary>
		/// 
		/// </summary>
		private string m_sLocalPath;
		/// <summary>
		/// 
		/// </summary>
		private string m_sLogPath;
		/// <summary>
		/// 
		/// </summary>
		private string m_sProject;
		/// <summary>
		/// 
		/// </summary>
		private string m_sLine;
		/// <summary>
		/// 
		/// </summary>
		private string m_dbTitle;
		/// <summary>
		/// 
		/// </summary>
		private string m_sApplicationName;
		/// <summary>
		/// 
		/// </summary>
		private string m_sPassword;
		/// <summary>
		/// 
		/// </summary>
		private int m_iLoginTime;
		/// <summary>
		/// 
		/// </summary>
		private int m_iPlcDriverPort;
		/// <summary>
		/// 
		/// </summary>
		private int m_iPlcDriverSize;
		/// <summary>
		/// 
		/// </summary>
		private string m_sPlcNoEventIndex;
		#endregion

		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public MainWindow MainWindow
		{
			get
			{
				return m_mainWindow;
			}
			set
			{
				m_mainWindow = value;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public PLCManager PLCManager
		{
			get
			{
				return m_plcManager;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public SvidManager SvidManager
		{
			get
			{
				return m_svidManager;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sLocalPath
		{
			get
			{
				return m_sLocalPath;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sConfigPath
		{
			get
			{
				return m_sConfigPath;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sProject
		{
			get
			{
				return m_sProject;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string sLine
		{
			get
			{
				return m_sLine;
			}
		}

		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		public Configurator(MainWindow mainControl)
		{
			m_mainWindow= mainControl;
			//m_plcManager = new PLCManager(this);
		}
		public bool Initialize(string sConfigPath, string sStartupPath)
		{
			try
			{
				bool bSuccess = false;

				string sStartup = string.Format("{0}\\{1}", sConfigPath, sStartupPath);

				XmlDocument doc = new XmlDocument();
				doc.Load(sStartup);
				
				if (!ReadConfigElement(sStartup))
				{
					MessageBox.Show("StartUp.xml 파일이 잘못되어 실행할 수 없습니다.", "에러", MessageBoxButton.OK, MessageBoxImage.Error);
					return false;
				}

				m_sConfigPath = sConfigPath;

				ReadConfiguration(doc);

				bSuccess = Initialize_Manager();

				return bSuccess;
			}
			catch (Exception ex)
			{

				return false;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private bool Initialize_Manager()
		{
			m_svidManager = new SvidManager(this);
			bool bSuccess = m_svidManager.Initialize(m_sUtilityPath);

			m_plcManager = new PLCManager(this);
			bSuccess = m_plcManager.Initialize(m_sDataStructPath, m_iPlcDriverPort, m_iPlcDriverSize, m_sPlcNoEventIndex);

			return bSuccess;
		}
		#endregion

		#region Class private methods
		#endregion

		#region Class public methods
		#endregion

		#region Class event handler
		#endregion

		#region class utility methods
		/// <summary>
		/// 
		/// </summary>
		private bool ReadConfigElement(string sConfigFile)
		{
			try
			{
				XmlReader reader = new XmlTextReader(sConfigFile);
				reader.Read();

				m_sProject = Common.ReadAttribute(reader, DEF_ATTR_PROJECT);
				m_sLine = Common.ReadAttribute(reader, DEF_ATTR_LINE);
				m_dbTitle = Common.ReadAttribute(reader, DEF_ATTR_DB);
				m_sApplicationName = Common.ReadAttribute(reader, DEF_ATTR_APPLICATION_NAME);
				m_sPassword = Common.ReadAttribute(reader, DEF_ATTR_PASSWORD);
				m_iLoginTime = Common.ReadAttributeAsInt(reader, DEF_ATTR_LOGIN);


				return true;
			}
			catch (Exception ex)
			{
				return false;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void ReadConfiguration(XmlDocument doc)
		{
			string sXPath = string.Format("//{0}/Utility", DEF_TOP_ELEMENT);
			ReadManagerElement(doc, sXPath, ref m_sUtilityPath);

			 sXPath = string.Format("//{0}/PlcDriver", DEF_TOP_ELEMENT);
			ReadManagerElement(doc, sXPath, ref m_sDataStructPath);

			m_iPlcDriverPort = Common.ReadAttributeAsInt(doc.SelectSingleNode(sXPath), "port");
			m_iPlcDriverSize = Common.ReadAttributeAsInt(doc.SelectSingleNode(sXPath), "size");
			m_sPlcNoEventIndex = Common.ReadAttribute(doc.SelectSingleNode(sXPath), "noevent");
		}
		/// <summary>
		/// 
		/// </summary>
		private void ReadManagerElement(XmlDocument doc, string sXPath, ref string sManagerConfigPath, bool bConfigPath = true)
		{
			try
			{
				XmlNode node = doc.SelectSingleNode(sXPath);

				if (null == node)
				{
					string sMessage = string.Format("Couldn't find element [{0}] within [{1}].",
						sXPath, m_sConfigPath);

					throw new Exception(sMessage);
				}

				string sConfigPath =
					Common.ReadAttribute(node, DEF_ATTR_PATH);

				sManagerConfigPath = (bConfigPath) ?
					GetConfigFolderRelativePath(m_sConfigPath, sConfigPath, m_sProject, m_sLine) : sConfigPath;

				if (string.IsNullOrEmpty(sManagerConfigPath))
				{
					string sMessage = string.Format(
						"Couldn't find [{0}] attribute within [{1}] element.",
						DEF_ATTR_PATH, sXPath);

					throw new Exception(sMessage);
				}
			}
			catch (Exception ex)
			{
				//LogManager.ErrorWriteLog(ex.ToString());
			}
		}
		/// <summary>
		/// 
		/// </summary>
		protected internal static string GetConfigFolderRelativePath(string sroot, string sConfigFile, string sProject, string sLine)
		{
			try
			{
				//250328 shkim sLine 폴더가 있는지 없는지 구분하는 방법. 
				DirectoryInfo diExist = new DirectoryInfo(string.Format("{0}\\{1}", sroot, sProject));
				if (diExist.GetDirectories().FirstOrDefault(x => x.Name.Contains(sLine)) is null)
				{
					DirectoryInfo di = new DirectoryInfo(string.Format("{0}\\{1}\\{2}", sroot, sProject, sConfigFile));
					return string.Format("{0}\\{1}\\{2}", sroot, sProject, sConfigFile);

				}
				else
				{
					DirectoryInfo di = new DirectoryInfo(string.Format("{0}\\{1}\\{2}\\{3}", sroot, sProject, sLine, sConfigFile));
					return string.Format("{0}\\{1}\\{2}\\{3}",sroot, sProject, sLine, sConfigFile);
				}

				//return string.Format("../ConfigFile/{0}/{1}/{2}", sProject, sConfigFile);
			}
			catch
			{
				return string.Empty;
			}
		}
		#endregion
	}
}
