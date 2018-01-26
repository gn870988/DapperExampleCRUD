namespace DapperHelper
{
    /// <summary>
    /// 連線字串
    /// </summary>
    public enum ConnectionSelecter
    {
        /// <summary>
        /// 測試機
        /// </summary>
        MsSqlTest,

        /// <summary>
        /// 正式機
        /// </summary>
        MsSqlLive,

        /// <summary>
        /// Sqlite
        /// </summary>
        SqlLiteLocalDb
    }
}
