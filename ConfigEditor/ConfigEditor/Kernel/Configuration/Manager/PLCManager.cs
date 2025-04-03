using JMKIM.XmlControl;
using Kornic.BlockControlFoundation.Drivers.Plc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace Kornic.BlockControlFoundation
{
	public class PLCManager
	{
		#region Class constants

		/// <summary>
		/// 
		/// </summary>
		public const string DEF_ELEMENT_NAME = "PlcStructs";
		/// <summary>
		/// 
		/// </summary>
		public const string DEF_ELEMENT_DEVICE= "Device";
		#endregion

		#region Class members

		private Configurator m_configurator;
		private PlcDriver m_plcDriver;
		/// <summary>
		/// 
		/// </summary>
		private XmlDataReader m_xmlReader;


		private object m_syncPlcObject;
		private bool m_bConnected;
		//public delegate void PlcEventHandler(PlcDataExchangeEventArgs ea);
		//public event PlcEventHandler PlcDataChanged;
		//public delegate void PlcConnectEventHandler(bool bConnect);
		//public event PlcConnectEventHandler PlcConnected;
		public delegate void PlcConfigrationChangetEventHandler(object sender, PlcConfigurationChangedEventArgs e);
		public event PlcConfigrationChangetEventHandler PlcConfigrationChanged;

		#endregion

		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public PlcDriver PlcDriver
		{
			get
			{
				return m_plcDriver;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public bool bConnected
		{
			get
			{
				return m_bConnected;
			}
		}
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		public PLCManager(Configurator configurator)
		{
			m_configurator = configurator;

			m_syncPlcObject = new object();

			CreateDataThread();

			m_plcDriver = new PlcDriver();
			//m_plcDriver.Connected += OnPlcDriverConnected;
			//m_plcDriver.Disconnected += OnPlcDriverDisconnected;
			//m_plcDriver.DataExchangeReport += OnPlcDriverDataExchangeReport;
			//m_plcDriver.ConfigurationChanged += OnPlcDriverConfigurationChanged;
		}
		/// <summary>
		/// 
		/// </summary>
		public bool Initialize(string sConfigFile, int iPort, int iSize, string sNoEvent)
		{
			try
			{
				//m_plcDriver.ConfigurationFolder = sConfigFile;
				m_plcDriver.ConfigurationFolder = Path.GetFullPath(string.Format(sConfigFile, iPort));

				return true;
			}
			catch(Exception ex)
			{
				return false;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		private void ReadXml()
		{
			try
			{
				//var hashData = m_xmlReader.XmlReadDictionary(DEF_ELEMENT_NAME, Enum.GetNames(typeof(eSVID)));

				//ReadDataToHash(hashData);
			}
			catch (Exception ex)
			{
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void ReadDataToHash(Dictionary<string, string[]> hashData)
		{
			try
			{
				string[] sKey = new string[hashData.Count];
				hashData.Keys.CopyTo(sKey, 0);

				for (int i = 0; i < sKey.Length; i++)
				{
					if (string.IsNullOrEmpty(sKey[i]))
					{
						continue;
					}
					ushort uKey = Convert.ToUInt16(sKey[i]);

					//	m_listTotalKeys.Add(uKey);

					//	var svid = new clSVID();
					//	svid.sPLC_NAME = hashData[sKey[i]][(int)eSVID.plcChannelName];
					//	svid.sFORMAT = hashData[sKey[i]][(int)eSVID.format];
					//	svid.sUNIT = hashData[sKey[i]][(int)eSVID.unit];
					//	svid.sRANGE = hashData[sKey[i]][(int)eSVID.range];
					//	svid.sLOCAL = hashData[sKey[i]][(int)eSVID.local];
					//	svid.sDOT = hashData[sKey[i]][(int)eSVID.dot];
					//	svid.sSIGNED = hashData[sKey[i]][(int)eSVID.signed];
					//	svid.sTYPE = hashData[sKey[i]][(int)eSVID.type];
					//	svid.bDOT_CUT = true;
					//	if (bool.TryParse(hashData[sKey[i]][(int)eSVID.dotCut], out bool bDotCut))
					//	{
					//		svid.bDOT_CUT = bDotCut;
					//	}

					//	m_hashSvidKeyToData[uKey] = svid;
					//	m_hashSvidKeyToPlcChannelName[uKey] = svid.sPLC_NAME;
					//}

					//m_listTotalKeys.Sort();
				}
			}
			catch (Exception ex)
			{
				//LogManager.ErrorWriteLog(ex.ToString());
			}
		}

		#endregion

		#region Class private methods
		/// <summary>
		/// 
		/// </summary>
		private void CreateDataThread()
		{
			try
			{
				//m_queuePlcBunchData = new Queue<PlcDataExchangeEventArgs>();
				//ThreadStart ts = new ThreadStart(OnPlcMessageProcessorThread);
				//m_threadPlcData = new Thread(ts);
				//m_threadPlcData.IsBackground = true;

				//m_threadPlcData.Start();
			}
			catch { }
		}
		#endregion

		#region Class public methods
		/// <summary>
		/// 
		/// <summary>
		/// 
		/// </summary>
		public PlcObject GetLocalObject(string sId, int iIndex = 0)
		{
			try
			{
				var ep = m_plcDriver
					.EndPoints
					.FirstOrDefault();

				if (null == ep)
					return null;

				var obj = ep
					.Prototypes
					.FirstOrDefault(x => x.Id == sId);

				return (null == obj) ? null : obj.Duplicate();
			}
			catch { return null; }
		}

		public int GetStructLength(string sId)
		{
			var plcObject = GetLocalObject(sId) as PlcWordStruct;

			if (plcObject != null)
				return ((PlcWordStruct)plcObject).SubObjects.Count;
			else return 0;
		}

		/// <summary>
		/// 
		/// </summary>
		public PlcObject GetLocalObject(int iLocal, string sName)
		{
			return GetLocalObject(string.Format("L{0}_{1}", iLocal, sName));
		}
		/// <summary>
		/// 
		/// </summary>
		public void WritePlcWordIndexIncrease(string sId)
		{
			try
			{
				PlcWord plcWord = GetLocalObject(sId) as PlcWord;


				if ((plcWord is PlcWord))
				{
					m_plcDriver.Read(plcWord, (ea) =>
					{
						if (m_plcDriver == null || ea.Objects == null)
							return;

						var res = ea.Objects.First() as PlcWord;
						int iValue = !string.IsNullOrEmpty(res.Value.ToString()) ? Convert.ToInt32(res.Value) : 0;
						iValue = iValue >= 32767 ? 0 : iValue;

						res.Value = (object)++iValue;
						m_plcDriver.Write(res);
					});
				}
			}
			catch (Exception ex)
			{

			}
		}
		/// <summary>
		/// 
		/// </summary>
		public int GetPlcStructLength(string sId)
		{
			PlcObject plcObject = GetLocalObject(sId);
			if (plcObject is PlcWordStruct)
			{
				PlcWordStruct plcWordStruct = plcObject as PlcWordStruct;
				return plcWordStruct.SubObjects.Count;

			}
			else
			{
				return 0;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public string PlcWordStructChecked(string sId)
		{
			try
			{
				PlcObject plcObject = this.GetLocalObject(sId);
				PlcWordStruct plcWordstruct = null;
				PlcWord plcWord = null;

				StringBuilder sb = new StringBuilder();

				if (plcObject is PlcWordStruct)
				{
					m_plcDriver.Read(plcObject, (ea) =>
					{
						if (m_plcDriver == null || ea.Objects == null)
							return;
						plcWordstruct = ea.Objects.FirstOrDefault() as PlcWordStruct;
					});
				}

				else if (plcObject is PlcWord)
				{
					m_plcDriver.Read(plcObject, (ea) =>
					{
						if (m_plcDriver == null || ea.Objects == null)
							return;
						plcWord = ea.Objects.FirstOrDefault() as PlcWord;
					});


				}

				Thread.Sleep(1);
				if (plcWordstruct != null)
				{

					foreach (var item in plcWordstruct.SubObjects)
					{
						string address = item.Device == PlcDeviceCode.W || item.Device == PlcDeviceCode.W ?
							item.Device + item.Address.ToString("X4") : item.Device + item.Address.ToString("D4");
						string sValue = item.Value.ToString().Trim('\0');
						sValue = string.IsNullOrEmpty(sValue) ? "" : sValue;

						sb.Append(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\r", item.Id.ToString(), address, item.Type.ToString(), item.Size.ToString(), sValue));

					}
				}

				if (plcWord != null)
				{
					string address = plcWord.Device == PlcDeviceCode.W || plcWord.Device == PlcDeviceCode.W ?
							plcWord.Device + plcWord.Address.ToString("X4") : plcWord.Device + plcWord.Address.ToString("D4");
					string sValue = plcWord.Value.ToString().Trim('\0');
					sValue = string.IsNullOrEmpty(sValue) ? "" : sValue;

					sb.Append(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\r", plcWord.Id.ToString(), address, plcWord.Type.ToString(), plcWord.Size.ToString(), sValue));

				}

				return sb.ToString();
			}
			catch (System.Exception ex)
			{
				//LogManager.ErrorWriteLog(ex.ToString());

				return string.Empty;
			}
		}
		#endregion

		#region Class event handler
		#endregion

		#region class utility methods
		#endregion
	}
}
