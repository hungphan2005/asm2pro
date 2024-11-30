namespace QuanLyBanDienThoai2
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            // Hiển thị LoginForm trước tiên
            using (var loginForm = new FormDangNhap())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    Application.Run(new Form1()); // Mở Form chính nếu đăng nhập thành công
                }
                else
                {
                    Application.Exit(); // Thoát chương trình nếu đóng FormDangNhap
                }
            }
        }
    }
}