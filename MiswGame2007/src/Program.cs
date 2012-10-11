using System;
using MiswGame2007;

public class Program
{
    public static void Main(string[] args)
    {
        try
        {
            using (MiswGame2007Application app = new MiswGame2007Application())
            {
                app.Run();
            }
        }
        catch (Exception e)
        {

#if DEBUG

            System.Windows.Forms.MessageBox.Show(e.Message + "\n\n↓スタックトレース\n" + e.StackTrace, "っうぇっうぇ", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Warning);

#else

            System.Windows.Forms.MessageBox.Show(e.Message);

#endif

        }
    }
}
