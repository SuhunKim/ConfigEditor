#region Usings
using JMKIM.XmlControl;
using Kornic.BlockControlFoundation;
using Kornic.BlockControlFoundation.Drivers.Plc;
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace ConfigEditor
{
	/// <summary>
	/// 
	/// </summary>
	public class SvidManager
	{
		#region class constants
		/// <summary>
		/// 
		/// </summary>
		public const string DEF_ELEMENT_NAME = "UtilityEntries";
		/// <summary>
		/// 
		/// </summary>
		public const string DEF_ELEMENT_SVID_ENTRY = "Utility";
		#endregion

		#region class members
		/// <summary>
		/// 
		/// </summary>
		private Configurator m_configurator;
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<int, string> m_hashSvidKeyToPlcChannelName;
		/// <summary>
		/// 
		/// </summary>
		//private Dictionary<int, string> m_hashSvidKeyToValue;
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<int, clSVID> m_hashSvidKeyToData;
		/// <summary>
		/// 
		/// </summary>
		private List<int> m_listTotalKeys;
		/// <summary>
		/// 
		/// </summary>
		private XmlDataReader m_xmlReader;
		/// <summary>
		/// 
		/// </summary>
		//private SvidLogManager m_svidLogManager;
		#endregion

		#region class properties
		/// <summary>
		/// 
		/// </summary>
		public Dictionary<int, string> hashSvidKeyToPlcChannelName
		{
			get
			{
				return m_hashSvidKeyToPlcChannelName;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public Dictionary<int, clSVID> hashSvidKeyToData
		{
			get
			{
				return m_hashSvidKeyToData;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public List<int> ListTotalKeys
		{
			get
			{
				return m_listTotalKeys;
			}
			set
			{
				foreach (int iValue in value)
				{
					if (!m_listTotalKeys.Contains(iValue))
					{
						m_listTotalKeys.Add(iValue);
					}
				}
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private PLCManager PLCManager
		{
			get
			{
				return m_configurator.PLCManager;
			}
		}
		#endregion

		#region class initialization
		/// <summary>
		/// 
		/// </summary>
		public SvidManager(Configurator configurator)
		{
			m_configurator = configurator;
			m_hashSvidKeyToPlcChannelName = new Dictionary<int, string>();
			m_hashSvidKeyToData = new Dictionary<int, clSVID>();
			m_listTotalKeys = new List<int>();


			//PlcDriverManager.PlcDataUpdated += OnPlcDataUpdated;
		}
		#endregion

		#region class 'Read configuration' methods
		/// <summary>
		/// 
		/// </summary>
		public bool Initialize(string sConfigFile)
		{
			try
			{
				m_xmlReader = new XmlDataReader(sConfigFile, false);

				ReadXml();

				return true;
			}
			catch 
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
				var hashData = m_xmlReader.XmlReadDictionary(DEF_ELEMENT_NAME, Enum.GetNames(typeof(eSVID)));

				ReadDataToHash(hashData);
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

					m_listTotalKeys.Add(uKey);

					var svid = new clSVID();
					svid.sPLC_NAME = hashData[sKey[i]][(int)eSVID.plcChannelName];
					svid.sFORMAT = hashData[sKey[i]][(int)eSVID.format];
					svid.sUNIT = hashData[sKey[i]][(int)eSVID.unit];
					svid.sRANGE = hashData[sKey[i]][(int)eSVID.range];
					svid.sLOCAL = hashData[sKey[i]][(int)eSVID.local];
					svid.sDOT = hashData[sKey[i]][(int)eSVID.dot];
					svid.sSIGNED = hashData[sKey[i]][(int)eSVID.signed];
					svid.sTYPE = hashData[sKey[i]][(int)eSVID.type];
					svid.bDOT_CUT = true;
					if (bool.TryParse(hashData[sKey[i]][(int)eSVID.dotCut], out bool bDotCut))
					{
						svid.bDOT_CUT = bDotCut;
					}

					m_hashSvidKeyToData[uKey] = svid;
					m_hashSvidKeyToPlcChannelName[uKey] = svid.sPLC_NAME;
				}

				m_listTotalKeys.Sort();
			}
			catch (Exception ex)
			{
				//LogManager.ErrorWriteLog(ex.ToString());
			}
		}
		#endregion

		#region class public methods
		#endregion

		#region class event handler
		/// <summary>
		/// 
		/// </summary>
		private void OnPlcDataUpdated(PlcDataExchangeEventArgs ea)
		{
			var listValue = ea.Objects.Where(x => x.Name == PlcName.DEF_FdcTrx);

			foreach (PlcObject plcObject in listValue)
			{
				//RegisterPlcDataChange(plcObject as PlcTransaction);
			}
		}
		#endregion

		#region class utility methods
		#endregion
	}
}