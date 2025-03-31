using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kornic.BlockControlFoundation
{
	public class PlcName
	{
		public const string DEF_ALARM_KEY = "ALARM";
		public const string DEF_ALARM_ITEM_HEADER = "Equipment,Time,Set,Code,Id,Unit,Alarm Text";
		public const string DEF_GLASS_KEY = "GLASS";
		public const string DEF_GLASS_ITEM_HEADER = "Equipment,Time,Type,FROM TO,POSITION,H_PANELID,E_PANELID,LOTID,BATCHID,JOBID,PORTID,SLOTID,PRODUCT_TYPE,PRODUCT_KIND,PRODUCTID,RUNSPECID,LAYERID,STEPID,PPID,FLOWID,GLASS_SIZE,GLASS_THICKNESS,GLASS_STATE,GLASS_ORDER,COMMENT,USE_COUNT,JUDGEMENT,REASON_CODE,INSPECTION_FLAG,ENC_FLAG,PRERUN_FLAG,TURN_DIR,FLIP_STATE,WORK_STATE,MULTI_USE,PAIR_GLASS,PAIR_PPID,OPTION_NAME1,OPTION_VALUE1,OPTION_NAME2,OPTION_VALUE2,OPTION_NAME3,OPTION_VALUE3,OPTION_NAME4,OPTION_VALUE4,OPTION_NAME5,OPTION_VALUE5,CSIF,AS,APS,UNIQUEID,BITSIGNAL";
		public const string DEF_PROCESS_KEY = "PROCESS";

		public const string DEF_FILE_LOG = "log";
		public const string DEF_FILE_CSV = "csv";

		public const string DEF_STARTUP_PATH = @"..\ConfigFile\";

		public const int DEF_REPORT_THREAD_SCAN_TIME = 500;
		public const int DEF_REPORT_NEXT_SLEEP_TIME = 100;

		public const string DEF_MCC_CONFIG_SYNC = "L1_B_MCC_CONFIG_SET_SYNC";
		public const string DEF_HOST_T1_TIME = "L1_W_HOST_TIMEOUT_T1";
		public const string DEF_HOST_T2_TIME = "L1_W_HOST_TIMEOUT_T2";
		public const string DEF_HOST_T3_TIME = "L1_W_HOST_TIMER_T3";


		public const string DEF_B_OPCALL_STATE = "B_OPCALL_STATE";

		public const string DEF_B_HOST_DATE_TIME_SET_COMMAND = "B_HOST_DATE_TIME_SET_COMMAND";
		public const string DEF_B_HOST_OPCALL_COMMAND = "B_HOST_OPCALL_COMMAND";
		public const string DEF_B_HOST_CONTROL_COMMAND = "B_HOST_CONTROL_COMMAND";
		public const string DEF_B_HOST_BUZZER_OFF_COMMAND = "B_HOST_BUZZER_OFF_COMMAND";
		public const string DEF_B_HOST_ECID_CHANGE_COMMAND = "B_HOST_ECID_CHANGE_COMMAND";
		public const string DEF_B_HOST_ECID_LIST_REQUEST_COMMAND = "B_HOST_ECID_LIST_REQUEST_COMMAND";
		public const string DEF_B_HOST_RECIPE_PROCESS_REQUEST_REPORT_REPLY = "B_HOST_RECIPE_PROCESS_REQUEST_REPORT_REPLY";


		public const string DEF_B_EQ_STATE_ALARM_EXIST = "B_EQ_STATE_ALARM_EXIST";
		public const string DEF_B_EQ_STATE_WARNING_EXIST = "B_EQ_STATE_WARNING_EXIST";
		public const string DEF_B_EQ_STATE_OPERATION_AUTO = "B_EQ_STATE_OPERATION_AUTO";
		public const string DEF_B_EQ_STATE_OPERATION_SEMI = "B_EQ_STATE_OPERATION_SEMI";
		public const string DEF_B_EQ_STATE_OPERATION_MANUAL = "B_EQ_STATE_OPERATION_MANUAL";


		public const string DEF_B_EQ_SIGNAL_BUZZER = "B_EQ_SIGNAL_BUZZER";
		public const string DEF_B_EQ_SIGNAL_RED = "B_EQ_SIGNAL_RED";
		public const string DEF_B_EQ_SIGNAL_YELLOW = "B_EQ_SIGNAL_YELLOW";
		public const string DEF_B_EQ_SIGNAL_GREEN = "B_EQ_SIGNAL_GREEN";
		public const string DEF_B_EQ_SIGNAL_BLUE = "B_EQ_SIGNAL_BLUE";

		public const string DEF_W_HOST_DATE_TIME_SET = "L1_W_HOST_DATE_TIME_SET";
		public const string DEF_W_HOST_TEST_MODE = "L1_W_HOST_TEST_MODE_{0:d2}";

		public const string DEF_HOST_OPCALL_STRUCT = "HOST_OPCALL_STRUCT";
		public const string DEF_HOST_CONTROL_STRUCT = "HOST_CONTROL_STRUCT";
		public const string DEF_HOST_ECID_CHANGE_COMMAND_INFO_STRUCT = "HOST_ECID_CHANGE_COMMAND_INFO_STRUCT";
		public const string DEF_HOST_ECID_CHANGE_COMMAND_DATA_STRUCT = "HOST_ECID_CHANGE_COMMAND_DATA_{0:d2}_STRUCT";
		public const string DEF_HOST_ECID_CHANGE_COMMAND_DATA_01_STRUCT = "HOST_ECID_CHANGE_COMMAND_DATA_01_STRUCT";
		public const string DEF_HOST_ECID_CHANGE_COMMAND_DATA_02_STRUCT = "HOST_ECID_CHANGE_COMMAND_DATA_02_STRUCT";
		public const string DEF_HOST_ECID_CHANGE_COMMAND_DATA_03_STRUCT = "HOST_ECID_CHANGE_COMMAND_DATA_03_STRUCT";
		public const string DEF_HOST_ECID_CHANGE_COMMAND_DATA_04_STRUCT = "HOST_ECID_CHANGE_COMMAND_DATA_04_STRUCT";
		public const string DEF_HOST_ECID_CHANGE_COMMAND_DATA_05_STRUCT = "HOST_ECID_CHANGE_COMMAND_DATA_05_STRUCT";


		public const string DEF_HOST_RECIPE_PROCESS_REQUEST_REPLY_STRUCT = "HOST_RECIPE_PROCESS_REQUEST_REPLY_STRUCT";
		public const string DEF_EQ_RECIPE_PROCESS_REQUEST_STRUCT = "EQ_RECIPE_PROCESS_REQUEST_STRUCT";

		public const string DEF_EQ_GLASS_POSITION_STRUCT = "EQ_GLASS_POSITION_STRUCT";

		public const string DEF_FdcTrx = "FdcTrx";
		public const string DEF_StatusSyncTrx = "StatusSyncTrx";


		// TRX
		public const string DEF_EqAlarmTrx = "EqAlarmTrx";
		public const string DEF_EqGlassInTrx = "EqGlassInTrx";
		public const string DEF_EqGlassOutTrx = "EqGlassOutTrx";
		public const string DEF_EqGlassScrapTrx = "EqGlassScrapTrx";
		public const string DEF_EqGlassDataChangeTrx = "EqGlassDataChangeTrx";
		public const string DEF_EqStatusChangeTrx = "EqStatusChangeTrx";
		public const string DEF_EqEcidChangeTrx = "EqEcidChangeTrx";
		public const string DEF_EqEcidListTrx = "EqEcidListTrx";
		public const string DEF_EqProcessDataTrx = "EqProcessDataTrx";
		public const string DEF_EqRecipeProcessTrx = "EqRecipeProcessTrx";


		// EOID 17
		public const string DEF_MccEoid17Trx = "MccEoid17Trx";
		public const string DEF_W_MCC_EOID_17_CHANGE = "L1_W_MCC_EOID_17_CHANGE";
		public const string DEF_CIM_EOID_17_STATE = "L1_B_CIM_EOID_17_STATE";
		public const string DEF_MCC_EOID_17_CHANGE = "L1_B_MCC_EOID_17_CHANGE";

		// AUTO MASK
		public const string DEF_JobChangeReadyTrx = "JobChangeReadyTrx";
		public const string DEF_APS_MASK_CHANGE_STRUCT = "L1_APS_MASK_CHANGE_STRUCT";
		public const string DEF_APS_JOB_CHANGE_READY = "L1_B_APS_JOB_CHANGE_READY";
		public const string DEF_CVD_JOB_CHANGE_READY_REPLY = "L1_B_CVD_JOB_CHANGE_REPLY";

		// REVERSE FLOW
		public const string DEF_ReverseFlowSyncTrx = "ReverseFlowTrx";
		public const string DEF_HOST_REVERSE_DATA_STRUCT = "L1_HOST_REVERSE_STRUCT";
		public const string DEF_HOST_REVERSE_MODE_STATE = "L1_B_HOST_REVERSE_MODE_STATE";
		public const string DEF_EQ_REVERSE_MODE_STATE = "L1_B_EQ_REVERSE_MODE_STATE";
		public const string DEF_REVERSE_FLOW_RESET = "L1_B_REVERSE_FLOW_RESET";

		// BUFFER
		public const string DEF_B_EQ_BUFFER_READY = "B_EQ_BUFFER_READY";
		public const string DEF_HOST_BUFFER_IMPORT_STRUCT = "HOST_BUFFER_IMPORT_STRUCT";
		public const string DEF_HOST_BUFFER_EXPORT_STRUCT = "HOST_BUFFER_EXPORT_STRUCT";

		//Control State
		public const string DEF_W_HOST_MCMD_STATE = "L1_W_HOST_MCMD_STATE";

	}
}