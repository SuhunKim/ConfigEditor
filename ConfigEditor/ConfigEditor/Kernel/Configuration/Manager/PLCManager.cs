using Kornic.BlockControlFoundation.Drivers.Plc;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Kornic.BlockControlFoundation
{
	public class PLCManager
	{
		#region Class constants
		#endregion

		#region Class members

		private Configurator m_configurator;
		private PlcDriver m_plcDriver;
		private object m_syncPlcObject;
		private Thread m_threadPlcData;
		private bool m_bConnected;
		private Dictionary<string, string[]> m_hashPlcCurrentValue;
		private Queue<PlcDataExchangeEventArgs> m_queuePlcBunchData;
		public delegate void PlcEventHandler(PlcDataExchangeEventArgs ea);
		public event PlcEventHandler PlcDataChanged;
		public delegate void PlcConnectEventHandler(bool bConnect);
		public event PlcConnectEventHandler PlcConnected;
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
		/// <summary>
		/// 
		/// </summary>
		public Dictionary<string, string[]> PlcCurrentValue
		{
			get
			{
				return m_hashPlcCurrentValue;
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
			m_hashPlcCurrentValue = new Dictionary<string, string[]>();

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
		public void Initialize(string sConfigFile, int iPort, int iSize, string sNoEvent)
		{
			//if (m_plcDriver.IsRunning)
			//{
			//	Stop();
			//}

			//m_plcDriver.ConfigurationFolder = sConfigFile;
			m_plcDriver.ConfigurationFolder = Path.GetFullPath(string.Format(sConfigFile, iPort));

			//Start();
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
		///// </summary>
		//public void Start()
		//{
		//	m_plcDriver.Start();
		//}
		///// <summary>
		///// 
		///// </summary>
		//public void Stop()
		//{
		//	m_plcDriver.Stop();
		//}
		/// <summary>
		/// 
		///// </summary>
		//public void Write(PlcObject plcObject)
		//{
		//	try
		//	{
		//		m_plcDriver.Write(plcObject);
		//	}
		//	catch { }
		//}
		///// <summary>
		///// 
		///// </summary>
		//public void Write(PlcObject[] plcObject)
		//{
		//	try
		//	{
		//		m_plcDriver.Write(plcObject);
		//	}
		//	catch { }
		//}
		///// <summary>
		///// 
		///// </summary>
		//public void WriteBit(int iLocal, string sName, bool bValue)
		//{
		//	WriteBit(string.Format("L{0}_{1}", iLocal, sName), bValue);
		//}
		///// <summary>
		///// 
		///// </summary>
		//public void WriteBit(string sId, bool bValue)
		//{
		//	PlcBit bit = GetLocalObject(sId) as PlcBit;
		//	bit.Value = bValue;
		//	Write(bit);
		//}
		///// <summary>
		///// 
		///// </summary>
		//public void WriteWord(int iLocal, string sName, object oValue)
		//{
		//	WriteWord(string.Format("L{0}_{1}", iLocal, sName), oValue);
		//}
		///// <summary>
		///// 
		///// </summary>
		//public void WriteWord(string sId, object oValue)
		//{
		//	PlcWord word = GetLocalObject(sId) as PlcWord;

		//	if (sId.Contains("BCD"))
		//	{
		//		oValue = GetBcdDateTime(oValue);
		//	}

		//	word.Value = oValue;
		//	Write(word);
		//}
		///// <summary>
		///// 
		///// </summary>
		//private int GetBcdDateTime(object oDateTime)
		//{
		//	try
		//	{
		//		string sDateTime = oDateTime.ToString();

		//		int iBCDData = 0;

		//		if (sDateTime.Length % 2 == 0)
		//		{
		//			int iCount = sDateTime.Length / 2;

		//			for (int i = 0; i < iCount; i++)
		//			{
		//				int iData = Convert.ToInt32(sDateTime.Substring(i * 2, 2), 16);

		//				iBCDData += iData << (8 * i);
		//			}
		//		}
		//		else
		//		{
		//			iBCDData = Convert.ToInt32(sDateTime);
		//		}

		//		return iBCDData;
		//	}
		//	catch
		//	{
		//		return 0;
		//	}
		//}
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

		//public int GetStructLength(string sId)
		//{
		//	var plcObject = GetLocalObject(sId) as PlcWordStruct;

		//	if (plcObject != null)
		//		return ((PlcWordStruct)plcObject).SubObjects.Count;
		//	else return 0;
		//}

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
		//public void WritePlcWordIndexIncrease(string sId)
		//{
		//	try
		//	{
		//		PlcWord plcWord = GetLocalObject(sId) as PlcWord;


		//		if ((plcWord is PlcWord))
		//		{
		//			m_plcDriver.Read(plcWord, (ea) =>
		//			{
		//				if (m_plcDriver == null || ea.Objects == null)
		//					return;

		//				var res = ea.Objects.First() as PlcWord;
		//				int iValue = !string.IsNullOrEmpty(res.Value.ToString()) ? Convert.ToInt32(res.Value) : 0;
		//				iValue = iValue >= 32767 ? 0 : iValue;

		//				res.Value = (object)++iValue;
		//				m_plcDriver.Write(res);
		//			});
		//		}
		//	}
		//	catch (Exception ex)
		//	{

		//	}
		//}
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
		//public string PlcWordStructChecked(string sId)
		//{
		//	try
		//	{
		//		PlcObject plcObject = this.GetLocalObject(sId);
		//		PlcWordStruct plcWordstruct = null;
		//		PlcWord plcWord = null;

		//		StringBuilder sb = new StringBuilder();

		//		if (plcObject is PlcWordStruct)
		//		{
		//			m_plcDriver.Read(plcObject, (ea) =>
		//			{
		//				if (m_plcDriver == null || ea.Objects == null)
		//					return;
		//				plcWordstruct = ea.Objects.FirstOrDefault() as PlcWordStruct;
		//			});
		//		}

		//		else if (plcObject is PlcWord)
		//		{
		//			m_plcDriver.Read(plcObject, (ea) =>
		//			{
		//				if (m_plcDriver == null || ea.Objects == null)
		//					return;
		//				plcWord = ea.Objects.FirstOrDefault() as PlcWord;
		//			});


		//		}

		//		Thread.Sleep(1);
		//		if (plcWordstruct != null)
		//		{

		//			foreach (var item in plcWordstruct.SubObjects)
		//			{
		//				string address = item.Device == PlcDeviceCode.W || item.Device == PlcDeviceCode.W ?
		//					item.Device + item.Address.ToString("X4") : item.Device + item.Address.ToString("D4");
		//				string sValue = item.Value.ToString().Trim('\0');
		//				sValue = string.IsNullOrEmpty(sValue) ? "" : sValue;

		//				sb.Append(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\r", item.Id.ToString(), address, item.Type.ToString(), item.Size.ToString(), sValue));

		//			}
		//		}

		//		if (plcWord != null)
		//		{
		//			string address = plcWord.Device == PlcDeviceCode.W || plcWord.Device == PlcDeviceCode.W ?
		//					plcWord.Device + plcWord.Address.ToString("X4") : plcWord.Device + plcWord.Address.ToString("D4");
		//			string sValue = plcWord.Value.ToString().Trim('\0');
		//			sValue = string.IsNullOrEmpty(sValue) ? "" : sValue;

		//			sb.Append(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\r", plcWord.Id.ToString(), address, plcWord.Type.ToString(), plcWord.Size.ToString(), sValue));

		//		}

		//		return sb.ToString();
		//	}
		//	catch (System.Exception ex)
		//	{
		//		//LogManager.ErrorWriteLog(ex.ToString());

		//		return string.Empty;
		//	}
		//}
		#endregion

		#region Class event handler
		//private void OnPlcDriverConnected(object sender, PlcEndPointEventArgs ea)
		//{
		//	try
		//	{
		//		if (PlcConnected != null)
		//			PlcConnected(true);

		//		m_bConnected = true;
		//	}
		//	catch { }
		//}
		//private void OnPlcDriverDisconnected(object sender, PlcEndPointEventArgs ea)
		//{
		//	try
		//	{
		//		if (PlcConnected != null)
		//			PlcConnected(false);

		//		m_bConnected = false;
		//	}
		//	catch { }
		//}
		//private void OnPlcDriverDataExchangeReport(object sender, PlcDataExchangeEventArgs ea)
		//{
		//	try
		//	{
		//		lock (m_syncPlcObject)
		//		{
		//			switch (ea.Type)
		//			{
		//				case PlcDataExchangeType.BatchRead:
		//				case PlcDataExchangeType.MultiBlockBatchRead:
		//				case PlcDataExchangeType.RandomRead:
		//					m_queuePlcBunchData.Enqueue(ea);
		//					break;

		//				default:
		//					return;
		//			}

		//			Monitor.Pulse(m_syncPlcObject);
		//		}
		//	}
		//	catch { }
		//}
		//private void OnPlcMessageProcessorThread()
		//{
		//	while (true)
		//	{
		//		PlcDataExchangeEventArgs args = new PlcDataExchangeEventArgs();
		//		int iCount = 0;

		//		try
		//		{
		//			lock (m_syncPlcObject)
		//			{
		//				if (m_queuePlcBunchData.Count > 0)
		//				{
		//					args = m_queuePlcBunchData.Dequeue();
		//					iCount = m_queuePlcBunchData.Count;
		//				}
		//				else
		//				{
		//					Monitor.Wait(m_syncPlcObject);
		//				}
		//			}

		//			if (args.Objects != null) OnPlcDataChanged(args);
		//		}
		//		catch { }
		//	}
		//}
		//private void OnPlcDriverConfigurationChanged(object sender, PlcConfigurationChangedEventArgs e)
		//{
		//	try
		//	{
		//		if (e.ConfigurationErrorCode != PlcErrorCode.None)
		//		{
		//			string sException = e.ConfigurationError.ToString();
		//		}

		//		if (PlcConfigrationChanged != null)
		//		{
		//			m_hashPlcCurrentValue.Clear();

		//			PlcConfigrationChanged(sender, e);
		//		}
		//	}
		//	catch { }
		//}
		//private void OnPlcDataChanged(PlcDataExchangeEventArgs ea)
		//{
		//	foreach (PlcObject plcobject in ea.Objects)
		//	{
		//		switch (plcobject.Type)
		//		{
		//			case PlcDataType.Bool:
		//				PlcBit plcbit = (PlcBit)plcobject;
		//				m_hashPlcCurrentValue[plcbit.Id] = new string[] { plcbit.Value.ToString() == "True" ? "1" : "0" };
		//				break;

		//			case PlcDataType.WordStruct:
		//				PlcWordStruct plcwordstruct = (PlcWordStruct)plcobject;
		//				string[] sArrValue = new string[GetPlcStructLength(plcwordstruct.Id) + 1];

		//				for (int i = 0, iLength = 0; i < plcwordstruct.Count; i++)
		//				{
		//					iLength++;
		//					sArrValue[iLength] = plcwordstruct[i].Value.ToString();

		//					foreach (PlcWordPart plcwordpart in plcwordstruct[i].Parts)
		//					{
		//						iLength++;
		//						sArrValue[iLength] = plcwordpart.Value.ToString();
		//					}
		//				}

		//				m_hashPlcCurrentValue[plcwordstruct.Id] = sArrValue;
		//				break;

		//			case PlcDataType.Transaction:
		//				break;

		//			default:
		//				PlcWord plcword = (PlcWord)plcobject;
		//				m_hashPlcCurrentValue[plcword.Id] = new string[] { plcword.Value.ToString() };
		//				break;
		//		}
		//	}

		//	PlcDataChanged(ea);
		//}

		#endregion

		#region class utility methods
		#endregion
	}
}
