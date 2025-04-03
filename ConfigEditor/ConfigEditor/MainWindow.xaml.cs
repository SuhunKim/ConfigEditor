using APS.WPF.SHEET;
using Kornic.BlockControlFoundation;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using unvell.ReoGrid;

namespace ConfigEditor
{
	/// <summary>
	/// 
	/// </summary>
	public partial class MainWindow : Window
	{
		#region Class constants
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_NOTE_PAD_PATH_32BIT = @"C:\Program Files\Notepad++\notepad++.exe";
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_NOTE_PAD_PATH_64BIT = @"C:\Program Files (x86)\Notepad++\notepad++.exe";
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_FILE_UTILITY = @"02_Utility";
		/// <summary>
		/// 
		/// </summary>
		private const string DEF_FILE_DATA_STRUCT = @"Data_Struct";

		/// <summary>
		/// 
		/// </summary>
		private const string DEF_ATTRIBUTE_KEY = "key";
		private const string DEF_ATTRIBUTE_NAME = "plcChannelName";
		private const string DEF_ATTRIBUTE_FORMAT = "format";
		private const string DEF_ATTRIBUTE_TYPE = "type";
		private const string DEF_ATTRIBUTE_UNIT = "unit";
		private const string DEF_ATTRIBUTE_RANGE = "range";
		private const string DEF_ATTRIBUTE_DOT = "dot";
		private const string DEF_ATTRIBUTE_SIGNED = "signed";
		private const string DEF_ATTRIBUTE_LOCAL = "local";
		/// <summary>
		/// 
		/// </summary>
		private const int DEF_COLUMN_KEY = 0;
		private const int DEF_COLUMN_NAME = 1;
		private const int DEF_COLUMN_FORMAT = 2;
		private const int DEF_COLUMN_TYPE = 3;
		private const int DEF_COLUMN_UNIT = 4;
		private const int DEF_COLUMN_RANGE = 5;
		private const int DEF_COLUMN_DOT = 6;
		private const int DEF_COLUMN_SIGN = 7;
		private const int DEF_COLUMN_LOCAL = 8;
		#endregion

		#region Class members

		private Configurator m_configurator;

		#endregion

		#region Class properties
		/// <summary>
		/// 
		/// </summary>
		public PLCManager PLCManager
		{
			get
			{
				return m_configurator.PLCManager;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		public SvidManager SvidManager
		{
			get
			{
				return m_configurator.SvidManager;
			}
		}
		#endregion

		#region Class initialization
		/// <summary>
		/// 
		/// </summary>
		public MainWindow()
		{
			InitializeComponent();

			m_configurator = new Configurator(this);

			Initialize();
		}

		/// <summary>
		/// 
		/// </summary>
		public void Initialize()
		{
			bool bSuccess = false;
			m_configurator = new Configurator(this);

			Initialize_Sheet();
			Initialize_Icon();
			UISetSheetBorder(m_spreadUtility);
		}
		/// <summary>
		/// 
		/// </summary>
		private bool Initialize_Sheet()
		{
			try
			{
				var hashSheet = new Dictionary<int, spColDefaultStyle>();

				var sp0 = new spColDefaultStyle(m_spreadUtility.DefaultStyle);
				sp0._dataFormat = unvell.ReoGrid.DataFormat.CellDataFormatFlag.Text;
				sp0._hAlign = unvell.ReoGrid.ReoGridHorAlign.Center;
				sp0._vAlign = unvell.ReoGrid.ReoGridVerAlign.Middle;
				sp0._indent = 2;
				sp0._IsReadOnly = false;
				hashSheet.Add(-1, sp0);

				var sp1 = new spColDefaultStyle(m_spreadUtility.DefaultStyle);
				sp1._dataFormat = unvell.ReoGrid.DataFormat.CellDataFormatFlag.Text;
				sp1._hAlign = unvell.ReoGrid.ReoGridHorAlign.Left;
				sp1._vAlign = unvell.ReoGrid.ReoGridVerAlign.Middle;
				sp1._indent = 2;
				sp1._IsReadOnly = false;
				hashSheet.Add(1, sp1);


				m_spreadUtility.Initialize(hashSheet);
				//m_spreadUtility.RowCount = 0;

				return true;
			}
			catch (Exception ex)
			{
				//LogManager.ErrorWriteLog(ex.ToString());

				return false;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void Initialize_Icon()
		{
			//var a = APS.ICON.APSImage.GetImageArray();
			//m_btnSelect.Content = APS.ICON.APSImage.GetIcon_Wpf("");
		}
		/// <summary>
		/// 
		/// </summary>
		private void UISetSheetBorder(APSSheet sheet)
		{
			sheet.SetBorders(0, 0, sheet.RowCount, sheet.ColumnCount, BorderPositions.InsideAll, RangeBorderStyle.SilverSolid);
		}
		#endregion

		#region Class private methods
		#endregion

		#region Class public methods

		#endregion

		#region Class event handler
		private void OnSelectClick(object sender, RoutedEventArgs e)
		{
			try
			{
				CommonOpenFileDialog Dialog = new CommonOpenFileDialog();

				Dialog.InitialDirectory = @"D:";
				Dialog.IsFolderPicker = true;
				Dialog.Title = "Select ConfigFile";

				if (Dialog.ShowDialog() == CommonFileDialogResult.Ok)
				{
					string m_sNewProjectFullPath = Dialog.FileName;

					if (!m_sNewProjectFullPath.Contains("ConfigFile"))
					{
						DirectoryInfo di = new DirectoryInfo(m_sNewProjectFullPath);
						DirectoryInfo sSubdi = di.GetDirectories().FirstOrDefault(x => x.Name.Contains("ConfigFile"));

						if (!(sSubdi is null) && sSubdi.Exists)
						{
							m_sNewProjectFullPath = sSubdi.FullName;
						}
					}

					if (!m_sNewProjectFullPath.Contains("ConfigFile"))
					{
						MessageBox.Show("ConfigFile을 선택해 주세요.");
						return;
					}

					bool bSuccess = m_configurator.Initialize(m_sNewProjectFullPath, "_StartUp.xml");

					if (bSuccess)
					{
						UISheetUtility();
					}
				}
			}
			catch (Exception)
			{

				throw;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void OnSaveClick(object sender, RoutedEventArgs e)
		{
			try
			{
				if (string.IsNullOrEmpty(m_configurator.sConfigPath))
				{
					return;
				}

				//Path 설정
				string sConfigPath = m_configurator.sConfigPath;
				string sXPath = string.Format("{0}\\{1}\\{2}", sConfigPath, m_configurator.sProject, m_configurator.sLine);
				string sUtilityPath = CreateFilePath(sXPath, DEF_FILE_UTILITY);

				sXPath = PLCManager.PlcDriver.ConfigurationFolder;
				sXPath = sXPath.EndsWith("\\") ? sXPath.Substring(0, sXPath.Length - 1) : sXPath;
				string sDataStructPath = string.Format("{0}\\{1}.xml",sXPath, DEF_FILE_DATA_STRUCT);  // CreateFilePath(sXPath, DEF_FILE_DATA_STRUCT);
				string sDataCopyPath = CreateFilePath(sXPath, DEF_FILE_DATA_STRUCT);

				//Dictionary<int, clSVID> hashKeyToData = CreateNewSVIDList();
				// Sheet Data to SVIDList
				Dictionary<string, clSVID> hashKeyToData = new Dictionary<string, clSVID>();
				Dictionary<string, List<clSVID>> hashLocalToSvid = new Dictionary<string, List<clSVID>>();

				for (int i = 0; i < m_spreadUtility.RowCount; i++)
				{
					var svid = new clSVID();
					svid.sId = m_spreadUtility.GetText(i, DEF_COLUMN_KEY);
					svid.sPLC_NAME = m_spreadUtility.GetText(i, DEF_COLUMN_NAME);
					svid.sFORMAT = m_spreadUtility.GetText(i, DEF_COLUMN_FORMAT);
					svid.sTYPE = m_spreadUtility.GetText(i, DEF_COLUMN_TYPE);
					svid.sUNIT = m_spreadUtility.GetText(i, DEF_COLUMN_UNIT);
					svid.sRANGE = m_spreadUtility.GetText(i, DEF_COLUMN_RANGE);
					svid.sDOT = m_spreadUtility.GetText(i, DEF_COLUMN_DOT);
					svid.sSIGNED = m_spreadUtility.GetText(i, DEF_COLUMN_SIGN);
					svid.sLOCAL = m_spreadUtility.GetText(i, DEF_COLUMN_LOCAL);
					svid.bDOT_CUT = true;

					if (bool.TryParse(m_spreadUtility.GetText(i, DEF_COLUMN_DOT), out bool bDotCut))
					{
						svid.bDOT_CUT = bDotCut;
					}

					hashKeyToData[svid.sId] = svid;
					if (hashLocalToSvid.ContainsKey(svid.sLOCAL))
					{
						hashLocalToSvid[svid.sLOCAL].Add(svid);
					}
					else
					{
						hashLocalToSvid.Add(svid.sLOCAL, new List<clSVID>() { svid });
					}
				}






				XmlWriterSettings settings = new XmlWriterSettings();
				settings.Indent = true;
				settings.IndentChars = "\t";
				settings.NewLineChars = "\r\n";
				settings.NewLineOnAttributes = true;

				//Utility 작성
				XmlWriter xmlwriter = XmlWriter.Create(sUtilityPath);
				xmlwriter.WriteStartDocument();
				xmlwriter.WriteWhitespace("\n");
				xmlwriter.WriteStartElement("UtilityEntries");

				//FDC
				foreach (var local in hashLocalToSvid)
				{
					//var a = hashLocalToSvid.FirstOrDefault().Value.FindAll(x => Convert.ToInt32(x.sId) < 60000);
					foreach (var svidItem in local.Value.FindAll(x => Convert.ToInt32(x.sId) < 60000))
					{
						xmlwriter.WriteWhitespace("\n\t");
						xmlwriter.WriteStartElement("Utility");
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_KEY,				svidItem.sId);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_NAME,			svidItem.sPLC_NAME);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_FORMAT,		svidItem.sFORMAT);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_TYPE,			svidItem.sTYPE);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_UNIT,			svidItem.sUNIT);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_RANGE,			svidItem.sRANGE);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_DOT,				svidItem.sDOT);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_SIGNED,		svidItem.sSIGNED);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_LOCAL,			svidItem.sLOCAL);
						xmlwriter.WriteEndElement();
					}
					xmlwriter.WriteWhitespace("\n\n");

				}
				//SEM
				foreach (var local in hashLocalToSvid)
				{
					foreach (var SEMItem in local.Value.FindAll(x => Convert.ToInt32(x.sId) > 60000))
					{
						xmlwriter.WriteWhitespace("\n\t");
						xmlwriter.WriteStartElement("Utility");
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_KEY,				SEMItem.sId);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_NAME,			SEMItem.sPLC_NAME);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_FORMAT,		SEMItem.sFORMAT);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_TYPE,			SEMItem.sTYPE);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_UNIT,			SEMItem.sUNIT);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_RANGE,			SEMItem.sRANGE);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_DOT,				SEMItem.sDOT);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_SIGNED,		SEMItem.sSIGNED);
						xmlwriter.WriteAttributeString(DEF_ATTRIBUTE_LOCAL, SEMItem.sLOCAL);
						xmlwriter.WriteEndElement();
					}
					xmlwriter.WriteWhitespace("\n\n");

				}

				xmlwriter.WriteEndDocument();
				xmlwriter.Flush();
				xmlwriter.Close();

				//250403  //PLC\Data_Struct\FDC_Struct 변환 코드
				XmlDocument doc = new XmlDocument();
				doc.Load(sDataStructPath);
				XmlNodeList itemNodes = doc.SelectNodes("PlcStructs/Device/PlcWordStruct");

				if (itemNodes is null)
				{
					//DataStruct 문서잘못됨 					//ERROR 표시 필요
				}

				foreach (XmlNode Node in itemNodes)
				{
					if (Node.Attributes.GetNamedItem("id").Value.ToString().Contains("FDC"))
					{
						XmlAttribute idAttributeStruct = doc.CreateAttribute("id");
						idAttributeStruct.Value = Node.Attributes.GetNamedItem("id").Value;		//"FDC".ToString();

						XmlAttribute typeAttributeStruct = doc.CreateAttribute("address");
						typeAttributeStruct.Value = Node.Attributes.GetNamedItem("address").Value;

						var temp = Node.ChildNodes;
						Node.RemoveAll();

						Node.Attributes.Append(idAttributeStruct);
						Node.Attributes.Append(typeAttributeStruct);

						for (int i = 0; i < 10; i++)
						{
							XmlElement newChild = doc.CreateElement("PlcWord");

							XmlAttribute idAttribute = doc.CreateAttribute("id");
							idAttribute.Value = i.ToString();
							newChild.Attributes.Append(idAttribute);

							XmlAttribute typeAttribute = doc.CreateAttribute("type");
							typeAttribute.Value = "I4";
							newChild.Attributes.Append(typeAttribute);

							Node.AppendChild(newChild);
						}
					}
				}
				doc.Save(sDataCopyPath);



			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK);
				return;
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private Dictionary<int, clSVID> CreateNewSVIDList()
		{
			// Sheet Data to SVIDList
			Dictionary<int, clSVID> hashSvidKeyToData = new Dictionary<int, clSVID>();

			for (int i = 0; i < m_spreadUtility.RowCount; i++)
			{
				var svid = new clSVID();
				int iSvid = Convert.ToInt32(m_spreadUtility.GetText(i, DEF_COLUMN_KEY));
				svid.sPLC_NAME = m_spreadUtility.GetText(i, DEF_COLUMN_NAME);
				svid.sFORMAT = m_spreadUtility.GetText(i, DEF_COLUMN_FORMAT);
				svid.sTYPE = m_spreadUtility.GetText(i, DEF_COLUMN_TYPE);
				svid.sUNIT = m_spreadUtility.GetText(i, DEF_COLUMN_UNIT);
				svid.sRANGE = m_spreadUtility.GetText(i, DEF_COLUMN_RANGE);
				svid.sDOT = m_spreadUtility.GetText(i, DEF_COLUMN_DOT);
				svid.sSIGNED = m_spreadUtility.GetText(i, DEF_COLUMN_SIGN);
				svid.sLOCAL = m_spreadUtility.GetText(i, DEF_COLUMN_LOCAL);
				svid.bDOT_CUT = true;

				if (bool.TryParse(m_spreadUtility.GetText(i, DEF_COLUMN_DOT), out bool bDotCut))
				{
					svid.bDOT_CUT = bDotCut;
				}

				hashSvidKeyToData[iSvid] = svid;
			}

			return hashSvidKeyToData;
		}

		/// <summary>
		/// 
		/// </summary>
		private string CreateFilePath(string sConfigPath, string sFileName)
		{
			DateTime time = DateTime.Now;
			string sDate = string.Format(@"_{0:d4}{1:d2}{2:d2}", time.Year, time.Month, time.Day);

			string sXFilePath = string.Format("{0}\\{1}{2}.xml", sConfigPath, sFileName, sDate);
			int iBackup = 1;

			while (File.Exists(sXFilePath))
			{
				sXFilePath = string.Format("{0}\\{1}{2}_({3}).xml", sConfigPath, sFileName, sDate, iBackup++.ToString());
			}

			return sXFilePath;
		}

		/// <summary>
		/// 
		/// </summary>
		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.O && Keyboard.Modifiers == ModifierKeys.Control)
			{
				OnSelectClick(null, null);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void OnAddRow(object sender, RoutedEventArgs e)
		{
			if (m_spreadUtility.RowCount > 20000)
			{
				return;
			}

			m_spreadUtility.RowCount += 1;
			//CrossThread.SheetRowCountCrossThread(m_spreadUtility, m_spreadUtility.RowCount + 1);
		}
		/// <summary>
		/// 
		/// </summary>
		private void OnInsertRow(object sender, RoutedEventArgs e)
		{
			if (m_spreadUtility.RowCount > 20000)
			{
				return;
			}

			int iRow = m_spreadUtility.CurrentWorksheet.FocusPos.Row;
			m_spreadUtility.CurrentWorksheet.InsertRows(iRow, 1);

		}
		/// <summary>
		/// 
		/// </summary>
		private void OnDeleteRow(object sender, RoutedEventArgs e)
		{
			int iRow = m_spreadUtility.CurrentWorksheet.FocusPos.Row;
			m_spreadUtility.CurrentWorksheet.DeleteRows(iRow, 1);
		}
		/// <summary>
		/// 
		/// </summary>
		private void OnNotePadClick(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrEmpty(m_configurator.sConfigPath))
			{
				return;
			}

			string sPath = string.Format("{0}\\{1}\\{2}.txt", m_configurator.sConfigPath, "CIM", "Utility");

			if (!File.Exists(sPath))
			{
				//MessageBoxForm messageForm = new MessageBoxForm(new Point(this.Location.X + 80, this.Location.Y + 180));
				//messageForm.ShowForm("Type is not File or The file doesn't exist.", "Confirm");
				if (File.Exists(DEF_NOTE_PAD_PATH_32BIT))
				{
					System.Diagnostics.Process.Start(DEF_NOTE_PAD_PATH_32BIT);
				}
				else if (File.Exists(DEF_NOTE_PAD_PATH_64BIT))
				{
					System.Diagnostics.Process.Start(DEF_NOTE_PAD_PATH_64BIT);
				}
			}
			else
			{
				System.Diagnostics.Process.Start(sPath);
			}
		}
		/// <summary>
		/// 
		/// </summary>
		private void UISheetUtility()
		{
			//string[] sKey = new string[SvidManager.hashKeyToData.Count];
			List<string[]> listFDC = new List<string[]>();

			m_spreadUtility.RowCount = SvidManager.hashSvidKeyToData.Count;// sKey.Length;
			int iSheetIndex = 0;

			foreach (var svid in SvidManager.hashSvidKeyToData)
			{
				StringBuilder sb = new StringBuilder();
				string sKey = svid.Key.ToString();
				string sName = svid.Value.sPLC_NAME;
				string sFormat = svid.Value.sFORMAT;
				string sType = svid.Value.sTYPE;
				string sUnit = svid.Value.sUNIT;
				string sRange = svid.Value.sRANGE;
				string sDot = svid.Value.sDOT;
				string sSigned = svid.Value.sSIGNED;
				string sLocal = svid.Value.sLOCAL;

				sb.Append(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\t{7}\t{8}\t", sKey, sName, sFormat, sType, sUnit, sRange, sDot, sSigned, sLocal));
				m_spreadUtility.SetClipValue(iSheetIndex++, 0, SvidManager.hashSvidKeyToData.Count, 9, sb.ToString());

				listFDC.Add(new string[] { svid.Key.ToString(), svid.Value.sPLC_NAME, svid.Value.sFORMAT, svid.Value.sTYPE, svid.Value.sUNIT, svid.Value.sRANGE,
					svid.Value.sDOT, svid.Value.sSIGNED, svid.Value.sLOCAL});
				//m_spreadUtility.SetClipValue(0, 0, listFDC);

			}
		}
		#endregion

		#region class utility methods
		/// <summary>
		/// 
		/// </summary>
		private void UIWindowMouseDown(object sender, MouseButtonEventArgs e)
		{
			if (e.ChangedButton == MouseButton.Left)
			{
				DragMove();
			}
		}
		#endregion

	}
}
