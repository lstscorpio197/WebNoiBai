using GemBox.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Razor.Tokenizer.Symbols;
using WebNoiBai.WHttpMessage;

namespace WebNoiBai.Common
{
    public class ExcelFiles
    {
        public HttpMessage ProcessFileExcel<T>(ExcelFile workbook, List<ExcelImport> LstExcelImport, ref List<T> LstResult, bool IsDDMMYYYY = true) where T : new()
        {
            HttpMessage httpMessage = new HttpMessage(false);
            try
            {
                SpreadsheetInfo.SetLicense(AppConst.KeyGemBoxSpreadsheet);
                var workSheet = workbook.Worksheets[0];
                int countRowsExcelFile = workSheet.Rows.Count;
                int rowStart = 2;
                DateTime dCurrent = DateTime.Now;
                int i = 0;
                for (i = rowStart - 1; i < countRowsExcelFile; i++)
                {
                    ExcelRow row = workSheet.Rows[i];
                    T itemDm = new T();
                    foreach (var propExcel in LstExcelImport)
                    {
                        string value = GetValuePropertyObject(row, propExcel.ExcelColumn);
                        var _typeColumn = propExcel.TypeColumn.ToLower();
                        switch (_typeColumn)
                        {
                            case "datetime":
                                DateTime? d = General.ConvertStringToDateTime(value, IsDDMMYYYY);
                                if (d == null && propExcel.IsBatBuoc)
                                {
                                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Row " + i + " column " + propExcel.MaColumn + "data fail");
                                    return httpMessage;
                                }
                                itemDm.GetType().GetProperty(propExcel.MaColumn).SetValue(itemDm, d, null);
                                break;
                            case "decimal":
                                decimal dValue = 0;
                                if (string.IsNullOrEmpty(value))
                                {
                                    value = "0";
                                }
                                if (!decimal.TryParse(value, out dValue))
                                {
                                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Row " + i + " column " + propExcel.MaColumn + "data fail");
                                    return httpMessage;
                                }
                                itemDm.GetType().GetProperty(propExcel.MaColumn).SetValue(itemDm, dValue, null);
                                break;
                            case "int":
                                int iValue = 0;
                                if (string.IsNullOrEmpty(value))
                                {
                                    value = "0";
                                }
                                if (!int.TryParse(value, out iValue))
                                {
                                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Row " + i + " column " + propExcel.MaColumn + "data fail");
                                    return httpMessage;
                                }
                                itemDm.GetType().GetProperty(propExcel.MaColumn).SetValue(itemDm, iValue, null);
                                break;
                            case "long":
                                long iValueLong = 0;
                                if (string.IsNullOrEmpty(value))
                                {
                                    value = "0";
                                }
                                if (!long.TryParse(value, out iValueLong))
                                {
                                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Row " + i + " column " + propExcel.MaColumn + "data fail");
                                    return httpMessage;
                                }
                                itemDm.GetType().GetProperty(propExcel.MaColumn).SetValue(itemDm, iValueLong, null);
                                break;
                            case "short":
                                short iValueShort = 0;
                                if (string.IsNullOrEmpty(value))
                                {
                                    value = "0";
                                }
                                if (!short.TryParse(value, out iValueShort))
                                {
                                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Row " + i + " column " + propExcel.MaColumn + "data fail");
                                    return httpMessage;
                                }
                                itemDm.GetType().GetProperty(propExcel.MaColumn).SetValue(itemDm, iValueShort, null);
                                break;
                            default:
                                value = (string.IsNullOrEmpty(value) ? "" : value.Trim());
                                value = value.Replace("'", "''");
                                itemDm.GetType().GetProperty(propExcel.MaColumn).SetValue(itemDm, value, null);
                                break;
                        }
                    }
                    LstResult.Add(itemDm);

                }
                httpMessage.IsOk = true;
                return httpMessage;
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return httpMessage;
            }
        }

        public HttpMessage ProcessFileExcelHK<chuyenbay_hanhkhach>(ExcelFile workbook, List<ExcelImport> LstExcelImport, ref List<chuyenbay_hanhkhach> LstResult, bool IsDDMMYYYY = true) where chuyenbay_hanhkhach : new()
        {
            HttpMessage httpMessage = new HttpMessage(false);
            try
            {
                SpreadsheetInfo.SetLicense(AppConst.KeyGemBoxSpreadsheet);
                var workSheet = workbook.Worksheets[0];
                int countRowsExcelFile = workSheet.Rows.Count;
                int rowStart = 2;
                DateTime dCurrent = DateTime.Now;
                int i = 0;
                for (i = rowStart - 1; i < countRowsExcelFile; i++)
                {
                    ExcelRow row = workSheet.Rows[i];
                    chuyenbay_hanhkhach itemDm = new chuyenbay_hanhkhach();
                    foreach (var propExcel in LstExcelImport)
                    {
                        string value = GetValuePropertyObject(row, propExcel.ExcelColumn);
                        var _typeColumn = propExcel.TypeColumn.ToLower();
                        switch (_typeColumn)
                        {
                            case "datetime":
                                DateTime? d = General.ConvertStringToDateTime(value, IsDDMMYYYY);
                                if (d == null && propExcel.IsBatBuoc)
                                {
                                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Row " + i + " column " + propExcel.MaColumn + "data fail");
                                    return httpMessage;
                                }
                                itemDm.GetType().GetProperty(propExcel.MaColumn).SetValue(itemDm, d, null);
                                break;
                            case "decimal":
                                decimal dValue = 0;
                                if (string.IsNullOrEmpty(value))
                                {
                                    value = "0";
                                }
                                if (!decimal.TryParse(value, out dValue))
                                {
                                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Row " + i + " column " + propExcel.MaColumn + "data fail");
                                    return httpMessage;
                                }
                                itemDm.GetType().GetProperty(propExcel.MaColumn).SetValue(itemDm, dValue, null);
                                break;
                            case "int":
                                int iValue = 0;
                                if (string.IsNullOrEmpty(value))
                                {
                                    value = "0";
                                }
                                if (!int.TryParse(value, out iValue))
                                {
                                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Row " + i + " column " + propExcel.MaColumn + "data fail");
                                    return httpMessage;
                                }
                                itemDm.GetType().GetProperty(propExcel.MaColumn).SetValue(itemDm, iValue, null);
                                break;
                            case "long":
                                long iValueLong = 0;
                                if (string.IsNullOrEmpty(value))
                                {
                                    value = "0";
                                }
                                if (!long.TryParse(value, out iValueLong))
                                {
                                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Row " + i + " column " + propExcel.MaColumn + "data fail");
                                    return httpMessage;
                                }
                                itemDm.GetType().GetProperty(propExcel.MaColumn).SetValue(itemDm, iValueLong, null);
                                break;
                            case "short":
                                short iValueShort = 0;
                                if (string.IsNullOrEmpty(value))
                                {
                                    value = "0";
                                }
                                if (!short.TryParse(value, out iValueShort))
                                {
                                    httpMessage.Body.MsgNoti = new HttpMessageNoti("400", null, "Row " + i + " column " + propExcel.MaColumn + "data fail");
                                    return httpMessage;
                                }
                                itemDm.GetType().GetProperty(propExcel.MaColumn).SetValue(itemDm, iValueShort, null);
                                break;
                            default:
                                value = (string.IsNullOrEmpty(value) ? "" : value.Trim());
                                itemDm.GetType().GetProperty(propExcel.MaColumn).SetValue(itemDm, value, null);
                                break;
                        }
                    }

                    LstResult.Add(itemDm);

                }
                httpMessage.IsOk = true;
                return httpMessage;
            }
            catch (Exception ex)
            {
                httpMessage.IsOk = false;
                httpMessage.Body.MsgNoti = new HttpMessageNoti("500", null, ex.Message);
                return httpMessage;
            }
        }

        public string GetValuePropertyObject(ExcelRow RowItem, string Prop, bool IsFormatNumbew = false)
        {
            try
            {
                if (RowItem == null) return string.Empty;
                if (IsFormatNumbew)
                {
                    decimal dValue = RowItem.Cells[Prop].Value == null ? 0 : decimal.Parse(RowItem.Cells[Prop].Value.ToString());
                    return string.Format("{0:#0.##}", dValue);
                }
                return RowItem.Cells[Prop].Value == null ? string.Empty : RowItem.Cells[Prop].Value.ToString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
    }
}