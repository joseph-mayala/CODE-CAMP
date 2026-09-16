using WinFormsApp.Data;

namespace WinFormsApp;

public partial class LoginForm : Form
{
    private Database _db;
    
    public LoginForm()
    {
        InitializeComponent();
        _db = new Database();
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        var username = txtUsername.Text;
        var password = txtPassword.Text;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Please enter username and password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var user = _db.Authenticate(username, password);
        if (user != null)
        {
            this.Hide();
            var dashboard = new DashboardForm(user, _db);
            dashboard.ShowDialog();
            this.Close();
        }
        else
        {
            MessageBox.Show("Invalid username or password", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}