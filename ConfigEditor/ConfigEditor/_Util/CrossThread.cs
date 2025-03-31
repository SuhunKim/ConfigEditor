#region Usings
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using APS.WPF.SHEET;
using unvell.ReoGrid;
#endregion

namespace Kornic.BlockControlFoundation
{
  public class CrossThread
  {
    #region cross button methods
    private delegate void ReloadButtonContent(Button btn, string sContent);
    /// <summary>
    /// 
    /// </summary>
    public static void ButtonContentCrossThread(Button btn, string sContent)
    {
      try
      {
        if (!btn.Dispatcher.CheckAccess())
        {
          ReloadButtonContent reload = new ReloadButtonContent(ButtonContentCrossThread);
          _ = btn.Dispatcher.BeginInvoke(reload, new object[] { btn, sContent });
        }
        else
        {
          btn.Content = sContent;
        }
      }
      catch { }
    }

    private delegate void ReloadButtonBackColor(Button btn, SolidColorBrush brush);
    /// <summary>
    /// 
    /// </summary>
    public static void ButtonBackColorCrossThread(Button btn, SolidColorBrush brush)
    {
      try
      {
        if (!btn.Dispatcher.CheckAccess())
        {
          ReloadButtonBackColor reload = new ReloadButtonBackColor(ButtonBackColorCrossThread);
          _ = btn.Dispatcher.BeginInvoke(reload, new object[] { btn, brush });
        }
        else
        {
          btn.Background = brush;
        }
      }
      catch { }
    }

    private delegate void ReloadButtonVisiable(Button btn, Visibility visible);
    /// <summary>
    /// 
    /// </summary>
    public static void ButtonVisiableCrossThread(Button btn, Visibility visible)
    {
      try
      {
        if (!btn.Dispatcher.CheckAccess())
        {
          ReloadButtonVisiable reload = new ReloadButtonVisiable(ButtonVisiableCrossThread);
          _ = btn.Dispatcher.BeginInvoke(reload, new object[] { btn, visible });
        }
        else
        {
          btn.Visibility = visible;
        }
      }
      catch { }
    }
    #endregion

    #region cross label methods
    private delegate void ReloadLabelContent(Label label, string sContent);
    /// <summary>
    /// 
    /// </summary>
    public static void LabelContentCrossThread(Label label, string sContent)
    {
      try
      {
        if (!label.Dispatcher.CheckAccess())
        {
          ReloadLabelContent reload = new ReloadLabelContent(LabelContentCrossThread);
          _ = label.Dispatcher.BeginInvoke(reload, new object[] { label, sContent });
        }
        else
        {
          label.Content = sContent;
        }
      }
      catch { }
    }
    #endregion

    #region cross imagebox methods
    private delegate void ReloadImageBoxBitmap(Image image, BitmapImage bitMap);
    /// <summary>
    /// 
    /// </summary>
    public static void ImageBoxCrossThread(Image image, BitmapImage bitMap)
    {
      try
      {
        if (!image.Dispatcher.CheckAccess())
        {
          ReloadImageBoxBitmap reload = new ReloadImageBoxBitmap(ImageBoxCrossThread);
          _ = image.Dispatcher.BeginInvoke(reload, new object[] { image, bitMap });
        }
        else
        {
          image.Source = bitMap;
        }
      }
      catch { }
    }
    #endregion

    #region cross textblock methods
    private delegate void ReloadTextBlockText(TextBlock label, string text);
    /// <summary>
    /// 
    /// </summary>
    public static void TextBlockCrossThread(TextBlock label, string text)
    {
      try
      {
        if (!label.Dispatcher.CheckAccess())
        {
          ReloadTextBlockText reload = new ReloadTextBlockText(TextBlockCrossThread);
          _ = label.Dispatcher.BeginInvoke(reload, new object[] { label, text });
        }
        else
        {
          label.Text = text;
        }
      }
      catch { }
    }

    private delegate void ReloadTextBlockColor(TextBlock label, Color color);
    /// <summary>
    /// 
    /// </summary>
    public static void TextBlockCrossThread(TextBlock label, Color color)
    {
      try
      {
        if (!label.Dispatcher.CheckAccess())
        {
          ReloadTextBlockColor reload = new ReloadTextBlockColor(TextBlockCrossThread);
          _ = label.Dispatcher.BeginInvoke(reload, new object[] { label, color });
        }
        else
        {
          label.Foreground = new SolidColorBrush(color);
        }
      }
      catch { }
    }
    #endregion

    #region cross listbox methods
    private delegate void ReloadListBoxItems(ListBox listbox, string sText, bool bScrollFix = false, int iMaxRow = 500, bool bClear = false);
    /// <summary>
    /// 
    /// </summary>
    public static void ListBoxCrossThread(ListBox listbox, string sText, bool bScrollFix = false, int iMaxRow = 500, bool bClear = false)
    {
      try
      {
        if (!listbox.Dispatcher.CheckAccess())
        {
          ReloadListBoxItems reload = new ReloadListBoxItems(ListBoxCrossThread);
          _ = listbox.Dispatcher.BeginInvoke(reload, new object[] { listbox, sText, bScrollFix, iMaxRow, bClear });
        }
        else
        {
          if (listbox.Items.Count > iMaxRow || bClear)
          {
            listbox.Items.Clear();
          }

          foreach (string text in sText.Split('\n'))
          {
            if(!string.IsNullOrEmpty(text))
              listbox.Items.Add(text.TrimEnd('\r'));
          }

          if (!bScrollFix)
          {
            if (VisualTreeHelper.GetChildrenCount(listbox) > 0)
            {
              Border border = VisualTreeHelper.GetChild(listbox, 0) as Border;
              if (border != null)
              {
                ScrollViewer sv = (ScrollViewer)VisualTreeHelper.GetChild(border, 0);
                sv.ScrollToBottom();
              }
            }
          }
        }
      }
      catch { }
    }
    #endregion

    #region cross progress bar methods
    private delegate void ReloadProgressValue(ProgressBar bar, double dValue);
    /// <summary>
    /// 
    /// </summary>
    public static void ProgressBarValueCrossThread(ProgressBar bar, double dValue)
    {
      try
      {
        if (!bar.Dispatcher.CheckAccess())
        {
          ReloadProgressValue reload = new ReloadProgressValue(ProgressBarValueCrossThread);
          _ = bar.Dispatcher.Invoke(reload, new object[] { bar, dValue });
        }
        else
        {
          bar.Value = dValue;
        }
      }
      catch { }
    }

    private delegate void ReloadProgressLimit(ProgressBar bar, double dMin, double dMax);
    /// <summary>
    /// 
    /// </summary>
    public static void ProgressLimitCrossThread(ProgressBar bar, double dMin, double dMax)
    {
      try
      {
        if (!bar.Dispatcher.CheckAccess())
        {
          ReloadProgressLimit reload = new ReloadProgressLimit(ProgressLimitCrossThread);
          _ = bar.Dispatcher.BeginInvoke(reload, new object[] { bar, dMin, dMax });
        }
        else
        {
          bar.Minimum = dMin;
          bar.Maximum = dMax;
        }
      }
      catch { }
    }
    #endregion

    #region cross border methods
    private delegate void ReloadBorderColor(Border border, SolidColorBrush brush);
    /// <summary>
    /// 
    /// </summary>
    public static void BorderColorCrossThread(Border border, SolidColorBrush brush)
    {
      try
      {
        if (!border.Dispatcher.CheckAccess())
        {
          ReloadBorderColor reload = new ReloadBorderColor(BorderColorCrossThread);
          _ = border.Dispatcher.BeginInvoke(reload, new object[] { border, brush });
        }
        else
        {
          border.Background = brush;
        }
      }
      catch { }
    }
    #endregion

    #region cross textbox methods
    private delegate void ReloadTextBoxString(TextBox text, string sValue);
    /// <summary>
    /// 
    /// </summary>
    public static void TextBoxStringCrossThread(TextBox text, string sValue)
    {
      try
      {
        if (!text.Dispatcher.CheckAccess())
        {
          ReloadTextBoxString reload = new ReloadTextBoxString(TextBoxStringCrossThread);
          _ = text.Dispatcher.BeginInvoke(reload, new object[] { text, sValue });
        }
        else
        {
          text.Text = sValue;
        }
      }
      catch { }
    }

    private delegate void ReloadTextBoxEnable(TextBox text, bool bEnable);
    /// <summary>
    /// 
    /// </summary>
    public static void TextBoxEnableCrossThread(TextBox text, bool bEnable)
    {
      try
      {
        if (!text.Dispatcher.CheckAccess())
        {
          ReloadTextBoxEnable reload = new ReloadTextBoxEnable(TextBoxEnableCrossThread);
          _ = text.Dispatcher.BeginInvoke(reload, new object[] { text, bEnable });
        }
        else
        {
          text.IsEnabled = bEnable;
        }
      }
      catch { }
    }
    #endregion

    #region cross spread methods
    private delegate void ReloadSpreadRowCount(APSSheet sheet, int iRowCount);
    /// <summary>
    /// 
    /// </summary>
    public static void SheetRowCountCrossThread(APSSheet sheet, int iRowCount)
    {
      try
      {
        if (!sheet.Dispatcher.CheckAccess())
        {
          ReloadSpreadRowCount reload = new ReloadSpreadRowCount(SheetRowCountCrossThread);
          _ = sheet.Dispatcher.Invoke(reload, new object[] { sheet, iRowCount });
        }
        else
        {
          sheet.RowCount = iRowCount;
        }
      }
      catch { }
    }

    private delegate void ReloadSpreadColCount(APSSheet sheet, int iColCount);
    /// <summary>
    /// 
    /// </summary>
    public static void SheetColCountCrossThread(APSSheet sheet, int iColCount)
    {
      try
      {
        if (!sheet.Dispatcher.CheckAccess())
        {
          ReloadSpreadColCount reload = new ReloadSpreadColCount(SheetColCountCrossThread);
          _ = sheet.Dispatcher.Invoke(reload, new object[] { sheet, iColCount });
        }
        else
        {
          sheet.ColumnCount = iColCount;
        }
      }
      catch { }
    }

    private delegate void ReloadSpreadClipValue(APSSheet sheet, int row, int col, int rowCount, int colCount, string sClip, bool bScrollUp);
    /// <summary>
    /// 
    /// </summary>
    public static void SheetClipValueCrossThread(APSSheet sheet, int row, int col, int rowCount, int colCount, string sClip, bool bScrollUp = false)
    {
      try
      {
        if (!sheet.Dispatcher.CheckAccess())
        {
          ReloadSpreadClipValue reload = new ReloadSpreadClipValue(SheetClipValueCrossThread);
          _ = sheet.Dispatcher.BeginInvoke(reload, new object[] { sheet, row, col, rowCount, colCount, sClip, bScrollUp });
        }
        else
        {
          sheet.SetClipValue(row, col, rowCount, colCount, sClip);

          if (bScrollUp)
          {
            sheet.ScrollCurrentWorksheet(0, 0);
          }
        }
      }
      catch { }
    }

    private delegate void ReloadSpreadClipList(APSSheet sheet, int row, int col, List<string[]> listValue, bool bScrollUp);
    /// <summary>
    /// 
    /// </summary>
    public static void SheetClipListCrossThread(APSSheet sheet, int row, int col, List<string[]> listValue, bool bScrollUp = false)
    {
      try
      {
        if (!sheet.Dispatcher.CheckAccess())
        {
          ReloadSpreadClipList reload = new ReloadSpreadClipList(SheetClipListCrossThread);
          _ = sheet.Dispatcher.BeginInvoke(reload, new object[] { sheet, row, col, listValue, bScrollUp });
        }
        else
        {
          sheet.SetClipValue(row, col, listValue);

          if (bScrollUp)
          {
            sheet.ScrollCurrentWorksheet(0, 0);
          }
        }
      }
      catch { }
    }

    private delegate void ReloadSpreadClipArray(APSSheet sheet, int row, int col, string[,] aryValue, bool bScrollUp);
    /// <summary>
    /// 
    /// </summary>
    public static void SheetClipArrayCrossThread(APSSheet sheet, int row, int col,string[,] aryValue, bool bScrollUp = false)
    {
      try
      {
        if (!sheet.Dispatcher.CheckAccess())
        {
          ReloadSpreadClipArray reload = new ReloadSpreadClipArray(SheetClipArrayCrossThread);
          _ = sheet.Dispatcher.Invoke(reload, new object[] { sheet, row, col, aryValue, bScrollUp });
        }
        else
        {
          sheet.SetClipValue(row, col, aryValue);

          if (bScrollUp)
          {
            sheet.ScrollCurrentWorksheet(0, 0);
          }
        }
      }
      catch { }
    }

    private delegate void ReloadSpreadColHeader(APSSheet sheet, int col, string sLabel, ushort iWidth);
    /// <summary>
    /// 
    /// </summary>
    public static void SheetColHeaderCrossThread(APSSheet sheet, int col, string sLabel, ushort iWidth = 0)
    {
      try
      {
        if (!sheet.Dispatcher.CheckAccess())
        {
          ReloadSpreadColHeader reload = new ReloadSpreadColHeader(SheetColHeaderCrossThread);
          _ = sheet.Dispatcher.Invoke(reload, new object[] { sheet, col, sLabel, iWidth });
        }
        else
        {
          if (sheet.ColumnCount > col)
          {
            sheet.SetColumnHeaderLabel(col, sLabel, iWidth);
          }
        }
      }
      catch { }
    }

    private delegate void ReloadSpreadUIUpdate(APSSheet sheet, bool bSuspened);
    /// <summary>
    /// 
    /// </summary>
    public static void SheetUiUpdateCrossThread(APSSheet sheet, bool bSuspend)
    {
      try
      {
        if (!sheet.Dispatcher.CheckAccess())
        {
          ReloadSpreadUIUpdate reload = new ReloadSpreadUIUpdate(SheetUiUpdateCrossThread);
          _ = sheet.Dispatcher.Invoke(reload, new object[] { sheet, bSuspend });
        }
        else
        {
          if (bSuspend)
          {
            sheet.SuspendUIUpdates();
          }
          else
          {
            sheet.ResumeUIUpdates();
          }
        }
      }
      catch { }
    }

    private delegate void ReloadSpreadSetText(APSSheet sheet, int row, int col, string sText);
    /// <summary>
    /// 
    /// </summary>
    public static void SheetSetTextCrossThread(APSSheet sheet, int row, int col, string sText)
    {
      try
      {
        if (!sheet.Dispatcher.CheckAccess())
        {
          ReloadSpreadSetText reload = new ReloadSpreadSetText(SheetSetTextCrossThread);
          _ = sheet.Dispatcher.BeginInvoke(reload, new object[] { sheet, row, col, sText });
        }
        else
        {
          sheet.SetText(row, col, sText);
        }
      }
      catch { }
    }

    private delegate void ReloadSpreadDataFormat(APSSheet sheet, unvell.ReoGrid.DataFormat.CellDataFormatFlag flag);
    /// <summary>
    /// 
    /// </summary>
    public static void SheetDataFormatCrossThread(APSSheet sheet, unvell.ReoGrid.DataFormat.CellDataFormatFlag flag)
    {
      try
      {
        if (!sheet.Dispatcher.CheckAccess())
        {
          ReloadSpreadDataFormat reload = new ReloadSpreadDataFormat(SheetDataFormatCrossThread);
          _ = sheet.Dispatcher.BeginInvoke(reload, new object[] { sheet, flag });
        }
        else
        {
          sheet.SetDataFormat(0, 0, sheet.RowCount, sheet.ColumnCount, flag);
        }
      }
      catch { }
    }

    private delegate void ReloadSpreadAlignCenter(APSSheet sheet);
    /// <summary>
    /// 
    /// </summary>
    public static void SheetAlignCenterCrossThread(APSSheet sheet)
    {
      try
      {
        if (!sheet.Dispatcher.CheckAccess())
        {
          ReloadSpreadAlignCenter reload = new ReloadSpreadAlignCenter(SheetAlignCenterCrossThread);
          _ = sheet.Dispatcher.BeginInvoke(reload, new object[] { sheet });
        }
        else
        {
          sheet.CurrentWorksheet.SetRangeStyles(0, 0, sheet.RowCount, sheet.ColumnCount, new WorksheetRangeStyle
          {
            Flag = PlainStyleFlag.AlignAll,
            HAlign = ReoGridHorAlign.Center,
            VAlign = ReoGridVerAlign.Middle,
          });
        }
      }
      catch { }
    }
    #endregion
  }
}
