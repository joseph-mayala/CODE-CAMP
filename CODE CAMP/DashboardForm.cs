using WinFormsApp.Models;
using WinFormsApp.Data;
using ClassModel = WinFormsApp.Models.Class;

namespace WinFormsApp;

public partial class DashboardForm : Form
{
    private User _currentUser;
    private Database _db;

    public DashboardForm(User user, Database db)
    {
        InitializeComponent();
        _currentUser = user;
        _db = db;
        LoadDashboard();
    }

    private void LoadDashboard()
    {
        this.Text = $"APU CodeCamp - {_currentUser.Role}: {_currentUser.FullName}";
        tabControl.TabPages.Clear();

        switch (_currentUser.Role)
        {
            case UserRole.Admin:
                CreateAdminTabs();
                break;
            case UserRole.Trainer:
                CreateTrainerTabs();
                break;
            case UserRole.Lecturer:
                CreateLecturerTabs();
                break;
            case UserRole.Student:
                CreateStudentTabs();
                break;
        }

        tabControl.TabPages.Add(tabProfile);
    }

    private void CreateAdminTabs()
    {
        var tabDashboard = new TabPage("Dashboard");
        tabDashboard.Controls.Add(CreateAdminDashboard());
        tabControl.TabPages.Add(tabDashboard);

        var tabTrainers = new TabPage("Manage Trainers");
        tabTrainers.Controls.Add(CreateAdminTrainersTab());
        tabControl.TabPages.Add(tabTrainers);

        var tabFeedback = new TabPage("Feedback");
        tabFeedback.Controls.Add(CreateAdminFeedbackTab());
        tabControl.TabPages.Add(tabFeedback);

        var tabIncome = new TabPage("Income Report");
        tabIncome.Controls.Add(CreateAdminIncomeTab());
        tabControl.TabPages.Add(tabIncome);
    }

    private void CreateTrainerTabs()
    {
        var tabClasses = new TabPage("My Classes");
        tabClasses.Controls.Add(CreateTrainerClassesTab());
        tabControl.TabPages.Add(tabClasses);

        var tabStudents = new TabPage("Enrolled Students");
        tabStudents.Controls.Add(CreateTrainerStudentsTab());
        tabControl.TabPages.Add(tabStudents);

        var tabFeedback = new TabPage("Send Feedback");
        tabFeedback.Controls.Add(CreateTrainerFeedbackTab());
        tabControl.TabPages.Add(tabFeedback);
    }

    private void CreateLecturerTabs()
    {
        var tabEnroll = new TabPage("Enroll Students");
        tabEnroll.Controls.Add(CreateLecturerEnrollTab());
        tabControl.TabPages.Add(tabEnroll);

        var tabRequests = new TabPage("Student Requests");
        tabRequests.Controls.Add(CreateLecturerRequestsTab());
        tabControl.TabPages.Add(tabRequests);

        var tabStudents = new TabPage("Student List");
        tabStudents.Controls.Add(CreateLecturerStudentsTab());
        tabControl.TabPages.Add(tabStudents);
    }

    private void CreateStudentTabs()
    {
        var tabSchedule = new TabPage("My Schedule");
        tabSchedule.Controls.Add(CreateStudentScheduleTab());
        tabControl.TabPages.Add(tabSchedule);

        var tabRequests = new TabPage("Request Coaching");
        tabRequests.Controls.Add(CreateStudentRequestsTab());
        tabControl.TabPages.Add(tabRequests);

        var tabPayments = new TabPage("Payments");
        tabPayments.Controls.Add(CreateStudentPaymentTab());
        tabControl.TabPages.Add(tabPayments);
    }

    private Panel CreateAdminDashboard()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var trainers = _db.GetTrainers();
        var classes = _db.GetClasses();
        var enrollments = _db.GetAllEnrollments();
        var feedback = _db.GetFeedback();

        var lblWelcome = new Label
        {
            Text = "Welcome to Admin Dashboard",
            Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(20, y),
            AutoSize = true
        };
        panel.Controls.Add(lblWelcome);
        y += 50;

        var lblInfo = new Label
        {
            Text = "Trainers: " + trainers.Count + "    Classes: " + classes.Count + "    Enrollments: " + enrollments.Count + "    Feedback: " + feedback.Count,
            Font = new System.Drawing.Font("Segoe UI", 12F),
            Location = new System.Drawing.Point(20, y),
            AutoSize = true
        };
        panel.Controls.Add(lblInfo);
        y += 50;

        var boxPanel = new FlowLayoutPanel
        {
            Location = new System.Drawing.Point(20, y),
            Width = 1000,
            Height = 200,
            FlowDirection = FlowDirection.TopDown
        };

        var trainerBox = CreateInfoBox("Trainers", trainers.Count.ToString(), System.Drawing.Color.LightBlue);
        var classBox = CreateInfoBox("Classes", classes.Count.ToString(), System.Drawing.Color.LightGreen);
        var enrollBox = CreateInfoBox("Enrollments", enrollments.Count.ToString(), System.Drawing.Color.LightYellow);
        var feedbackBox = CreateInfoBox("Feedback", feedback.Count.ToString(), System.Drawing.Color.LightPink);

        boxPanel.Controls.Add(trainerBox);
        boxPanel.Controls.Add(classBox);
        boxPanel.Controls.Add(enrollBox);
        boxPanel.Controls.Add(feedbackBox);
        panel.Controls.Add(boxPanel);

        return panel;
    }

    private Panel CreateInfoBox(string title, string value, System.Drawing.Color backColor)
    {
        var box = new Panel
        {
            Width = 220,
            Height = 80,
            BackColor = backColor,
            BorderStyle = BorderStyle.FixedSingle
        };

        var titleLabel = new Label
        {
            Text = title,
            Font = new System.Drawing.Font("Segoe UI", 10F),
            Location = new System.Drawing.Point(10, 10),
            AutoSize = true
        };

        var valueLabel = new Label
        {
            Text = value,
            Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(10, 35),
            AutoSize = true
        };

        box.Controls.Add(titleLabel);
        box.Controls.Add(valueLabel);
        return box;
    }

    private Panel CreateAdminTrainersTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var lblAdd = new Label { Text = "Add New Trainer:", Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold), Location = new System.Drawing.Point(20, y), AutoSize = true };
        panel.Controls.Add(lblAdd);
        y += 35;

        var lblName = new Label { Text = "Name:", Location = new System.Drawing.Point(20, y), Width = 60 };
        panel.Controls.Add(lblName);
        var txtName = new TextBox { Location = new System.Drawing.Point(85, y), Width = 180 };
        panel.Controls.Add(txtName);

        var lblUsername = new Label { Text = "Username:", Location = new System.Drawing.Point(280, y), Width = 80 };
        panel.Controls.Add(lblUsername);
        var txtUsername = new TextBox { Location = new System.Drawing.Point(365, y), Width = 150 };
        panel.Controls.Add(txtUsername);

        var lblEmail = new Label { Text = "Email:", Location = new System.Drawing.Point(530, y), Width = 60 };
        panel.Controls.Add(lblEmail);
        var txtEmail = new TextBox { Location = new System.Drawing.Point(595, y), Width = 200 };
        panel.Controls.Add(txtEmail);

        var lblContact = new Label { Text = "Contact:", Location = new System.Drawing.Point(810, y), Width = 70 };
        panel.Controls.Add(lblContact);
        var txtContact = new TextBox { Location = new System.Drawing.Point(885, y), Width = 150 };
        panel.Controls.Add(txtContact);
        y += 35;

        var btnAdd = new Button { Text = "Add Trainer", Location = new System.Drawing.Point(85, y), Width = 120, Height = 35 };
        btnAdd.Click += (s, e) =>
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtUsername.Text))
            {
                MessageBox.Show("Name and username required");
                return;
            }
            _db.AddTrainer(new User { FullName = txtName.Text, Username = txtUsername.Text, Email = txtEmail.Text, Contact = txtContact.Text });
            MessageBox.Show("Trainer added!");
            RefreshDashboard();
        };
        panel.Controls.Add(btnAdd);
        y += 50;

        var lblTrainers = new Label { Text = "Trainers List:", Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold), Location = new System.Drawing.Point(20, y), AutoSize = true };
        panel.Controls.Add(lblTrainers);
        y += 30;

        var dgvTrainers = new DataGridView
        {
            Location = new System.Drawing.Point(20, y),
            Width = 1050,
            Height = 180,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        dgvTrainers.Columns.Add("ID", "ID");
        dgvTrainers.Columns.Add("Name", "Name");
        dgvTrainers.Columns.Add("Username", "Username");
        dgvTrainers.Columns.Add("Email", "Email");
        dgvTrainers.Columns.Add("Contact", "Contact");

        var trainers = _db.GetTrainers();
        foreach (var t in trainers)
        {
            dgvTrainers.Rows.Add(t.Id, t.FullName, t.Username, t.Email ?? "", t.Contact ?? "");
        }
        panel.Controls.Add(dgvTrainers);
        y += 190;

        var btnDelete = new Button { Text = "Delete Selected Trainer", Location = new System.Drawing.Point(20, y), Width = 160, Height = 35 };
        btnDelete.Click += (s, e) =>
        {
            if (dgvTrainers.SelectedRows.Count > 0)
            {
                var id = Convert.ToInt32(dgvTrainers.SelectedRows[0].Cells[0].Value);
                _db.DeleteUser(id);
                RefreshDashboard();
            }
        };
        panel.Controls.Add(btnDelete);
        y += 50;

        var lblAssign = new Label { Text = "Assign Trainer to Class:", Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold), Location = new System.Drawing.Point(20, y), AutoSize = true };
        panel.Controls.Add(lblAssign);
        y += 30;

        var lblSelectClass = new Label { Text = "Class:", Location = new System.Drawing.Point(20, y), Width = 60 };
        panel.Controls.Add(lblSelectClass);
        var classes = _db.GetClasses();
        var cmbClasses = new ComboBox { Location = new System.Drawing.Point(85, y), Width = 280 };
        foreach (var c in classes) cmbClasses.Items.Add(c.Id + ": " + c.ModuleName + " (" + c.Level + ")");
        if (classes.Count > 0) cmbClasses.SelectedIndex = 0;
        panel.Controls.Add(cmbClasses);

        var lblSelectTrainer = new Label { Text = "Trainer:", Location = new System.Drawing.Point(380, y), Width = 70 };
        panel.Controls.Add(lblSelectTrainer);
        var cmbTrainers = new ComboBox { Location = new System.Drawing.Point(455, y), Width = 250 };
        foreach (var t in trainers) cmbTrainers.Items.Add(t.Id + ": " + t.FullName);
        if (trainers.Count > 0) cmbTrainers.SelectedIndex = 0;
        panel.Controls.Add(cmbTrainers);

        var btnAssign = new Button { Text = "Assign Trainer", Location = new System.Drawing.Point(720, y), Width = 140, Height = 30 };
        btnAssign.Click += (s, e) =>
        {
            if (cmbClasses.SelectedIndex >= 0 && cmbTrainers.SelectedIndex >= 0 && classes.Count > 0 && trainers.Count > 0)
            {
                var selectedClass = classes[cmbClasses.SelectedIndex];
                var selectedTrainer = trainers[cmbTrainers.SelectedIndex];
                _db.AssignTrainerToClass(selectedClass.Id, selectedTrainer.Id, selectedTrainer.FullName);
                MessageBox.Show("Trainer assigned to class!");
                RefreshDashboard();
            }
        };
        panel.Controls.Add(btnAssign);

        return panel;
    }

    private Panel CreateAdminFeedbackTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var lblTitle = new Label
        {
            Text = "Trainer Feedback",
            Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(20, y),
            AutoSize = true
        };
        panel.Controls.Add(lblTitle);
        y += 40;

        var dgvFeedback = new DataGridView
        {
            Location = new System.Drawing.Point(20, y),
            Width = 1050,
            Height = 400,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true
        };
        dgvFeedback.Columns.Add("ID", "ID");
        dgvFeedback.Columns.Add("Trainer", "Trainer");
        dgvFeedback.Columns.Add("Message", "Message");
        dgvFeedback.Columns.Add("Date", "Date");

        var feedback = _db.GetFeedback();
        foreach (var f in feedback)
        {
            dgvFeedback.Rows.Add(f.Id, f.TrainerName ?? "", f.Message ?? "", f.CreatedAt.ToString("yyyy-MM-dd"));
        }
        panel.Controls.Add(dgvFeedback);
        y += 420;

        var btnRefresh = new Button { Text = "Refresh", Location = new System.Drawing.Point(20, y), Width = 120, Height = 35 };
        btnRefresh.Click += (s, e) => RefreshDashboard();
        panel.Controls.Add(btnRefresh);

        return panel;
    }

    private Panel CreateAdminIncomeTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var lblTitle = new Label
        {
            Text = "Monthly Income Report",
            Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(20, y),
            AutoSize = true
        };
        panel.Controls.Add(lblTitle);
        y += 45;

        var lblMonth = new Label { Text = "Month:", Location = new System.Drawing.Point(20, y), Width = 60 };
        panel.Controls.Add(lblMonth);

        var cmbMonth = new ComboBox { Location = new System.Drawing.Point(85, y), Width = 150 };
        string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        foreach (var m in months) cmbMonth.Items.Add(m);
        cmbMonth.SelectedIndex = DateTime.Now.Month - 1;
        panel.Controls.Add(cmbMonth);

        var lblYear = new Label { Text = "Year:", Location = new System.Drawing.Point(260, y), Width = 50 };
        panel.Controls.Add(lblYear);

        var numYear = new NumericUpDown { Location = new System.Drawing.Point(315, y), Width = 100, Minimum = 2020, Maximum = 2030, Value = DateTime.Now.Year };
        panel.Controls.Add(numYear);
        y += 40;

        var btnShow = new Button { Text = "Show Report", Location = new System.Drawing.Point(85, y), Width = 120, Height = 35 };
        panel.Controls.Add(btnShow);
        y += 50;

        var txtResult = new TextBox
        {
            Location = new System.Drawing.Point(20, y),
            Width = 500,
            Height = 350,
            Multiline = true,
            ReadOnly = true,
            ScrollBars = ScrollBars.Vertical,
            Font = new System.Drawing.Font("Consolas", 11F)
        };
        btnShow.Click += (s, e) =>
        {
            var month = (cmbMonth.SelectedIndex + 1).ToString("D2");
            var year = numYear.Value.ToString();
            var monthStr = year + "-" + month;

            var trainers = _db.GetTrainers();
            var result = "Income Report for " + months[cmbMonth.SelectedIndex] + " " + year + ":\n\n";
            decimal total = 0;

            foreach (var t in trainers)
            {
                var income = _db.GetIncomeByTrainer(t.Id, monthStr);
                result = result + t.FullName + ": RM " + income.ToString("F2") + "\n";
                total = total + income;
            }
            result = result + "\nTotal: RM " + total.ToString("F2");
            txtResult.Text = result;
        };
        panel.Controls.Add(txtResult);

        return panel;
    }

    private Panel CreateTrainerClassesTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var lblAdd = new Label { Text = "Add New Class:", Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold), Location = new System.Drawing.Point(20, y), AutoSize = true };
        panel.Controls.Add(lblAdd);
        y += 35;

        var lblModule = new Label { Text = "Module:", Location = new System.Drawing.Point(20, y), Width = 70 };
        panel.Controls.Add(lblModule);
        var txtModule = new TextBox { Location = new System.Drawing.Point(95, y), Width = 200 };
        panel.Controls.Add(txtModule);

        var lblLevel = new Label { Text = "Level:", Location = new System.Drawing.Point(310, y), Width = 60 };
        panel.Controls.Add(lblLevel);
        var cmbLevel = new ComboBox { Location = new System.Drawing.Point(375, y), Width = 130 };
        cmbLevel.Items.AddRange(new[] { "Beginner", "Intermediate", "Advance" });
        cmbLevel.SelectedIndex = 0;
        panel.Controls.Add(cmbLevel);

        var lblCharges = new Label { Text = "Charges:", Location = new System.Drawing.Point(520, y), Width = 70 };
        panel.Controls.Add(lblCharges);
        var txtCharges = new TextBox { Location = new System.Drawing.Point(595, y), Width = 100 };
        panel.Controls.Add(txtCharges);

        var lblSchedule = new Label { Text = "Schedule:", Location = new System.Drawing.Point(710, y), Width = 80 };
        panel.Controls.Add(lblSchedule);
        var txtSchedule = new TextBox { Location = new System.Drawing.Point(795, y), Width = 200 };
        panel.Controls.Add(txtSchedule);
        y += 35;

        var btnAdd = new Button { Text = "Add Class", Location = new System.Drawing.Point(95, y), Width = 130, Height = 35 };
        btnAdd.Click += (s, e) =>
        {
            if (string.IsNullOrEmpty(txtModule.Text) || string.IsNullOrEmpty(txtCharges.Text))
            {
                MessageBox.Show("Module name and charges required");
                return;
            }
            _db.AddClass(new Class
            {
                ModuleName = txtModule.Text,
                Level = cmbLevel.SelectedItem?.ToString() ?? "Beginner",
                Charges = decimal.Parse(txtCharges.Text),
                Schedule = txtSchedule.Text,
                TrainerId = _currentUser.Id,
                TrainerName = _currentUser.FullName
            });
            MessageBox.Show("Class added!");
            RefreshDashboard();
        };
        panel.Controls.Add(btnAdd);
        y += 50;

        var lblClasses = new Label { Text = "My Classes:", Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold), Location = new System.Drawing.Point(20, y), AutoSize = true };
        panel.Controls.Add(lblClasses);
        y += 30;

        var dgvClasses = new DataGridView
        {
            Location = new System.Drawing.Point(20, y),
            Width = 1050,
            Height = 250,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        dgvClasses.Columns.Add("ID", "ID");
        dgvClasses.Columns.Add("Module", "Module");
        dgvClasses.Columns.Add("Level", "Level");
        dgvClasses.Columns.Add("Charges", "Charges");
        dgvClasses.Columns.Add("Schedule", "Schedule");

        var allClasses = _db.GetClasses();
        var classes = new List<Class>();
        for (int i = 0; i < allClasses.Count; i++)
        {
            if (allClasses[i].TrainerId == _currentUser.Id)
            {
                classes.Add(allClasses[i]);
            }
        }
        for (int i = 0; i < classes.Count; i++)
        {
            dgvClasses.Rows.Add(classes[i].Id, classes[i].ModuleName, classes[i].Level, "RM " + classes[i].Charges.ToString("F2"), classes[i].Schedule ?? "");
        }
        panel.Controls.Add(dgvClasses);
        y += 260;

        var btnDelete = new Button { Text = "Delete Selected Class", Location = new System.Drawing.Point(20, y), Width = 160, Height = 35 };
        btnDelete.Click += (s, e) =>
        {
            if (dgvClasses.SelectedRows.Count > 0)
            {
                var id = Convert.ToInt32(dgvClasses.SelectedRows[0].Cells[0].Value);
                _db.DeleteClass(id);
                RefreshDashboard();
            }
        };
        panel.Controls.Add(btnDelete);

        return panel;
    }

    private Panel CreateTrainerStudentsTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var lblTitle = new Label
        {
            Text = "Enrolled Students (Paid Only)",
            Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(20, y),
            AutoSize = true
        };
        panel.Controls.Add(lblTitle);
        y += 35;

        var lblNote = new Label { Text = "Only students who have paid are shown", Location = new System.Drawing.Point(20, y), ForeColor = System.Drawing.Color.Gray };
        panel.Controls.Add(lblNote);
        y += 30;

        var dgvStudents = new DataGridView
        {
            Location = new System.Drawing.Point(20, y),
            Width = 1050,
            Height = 350,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        dgvStudents.Columns.Add("ID", "ID");
        dgvStudents.Columns.Add("Student Name", "Student Name");
        dgvStudents.Columns.Add("TP Number", "TP Number");
        dgvStudents.Columns.Add("Module", "Module");
        dgvStudents.Columns.Add("Level", "Level");
        dgvStudents.Columns.Add("Month", "Month");

        var enrollments = _db.GetEnrollmentsForTrainer(_currentUser.Id);
        foreach (var e in enrollments)
        {
            dgvStudents.Rows.Add(e.Id, e.StudentName ?? "", e.TPNumber ?? "", e.ModuleName ?? "", e.Level ?? "", e.EnrolledMonth ?? "");
        }
        panel.Controls.Add(dgvStudents);
        y += 370;

        var btnComplete = new Button { Text = "Mark Completed", Location = new System.Drawing.Point(20, y), Width = 140, Height = 35 };
        btnComplete.Click += (s, e) =>
        {
            if (dgvStudents.SelectedRows.Count > 0)
            {
                var id = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells[0].Value);
                _db.MarkEnrollmentCompleted(id);
                RefreshDashboard();
            }
        };
        panel.Controls.Add(btnComplete);

        return panel;
    }

    private Panel CreateTrainerFeedbackTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var lblTitle = new Label
        {
            Text = "Send Feedback to Admin",
            Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(20, y),
            AutoSize = true
        };
        panel.Controls.Add(lblTitle);
        y += 35;

        var lblMsg = new Label { Text = "Submit suggestions or complaints to the administrator:", Location = new System.Drawing.Point(20, y) };
        panel.Controls.Add(lblMsg);
        y += 30;

        var txtMessage = new TextBox
        {
            Location = new System.Drawing.Point(20, y),
            Width = 800,
            Height = 300,
            Multiline = true,
            ScrollBars = ScrollBars.Vertical
        };
        panel.Controls.Add(txtMessage);
        y += 320;

        var btnSend = new Button { Text = "Send Feedback", Location = new System.Drawing.Point(20, y), Width = 140, Height = 35 };
        btnSend.Click += (s, e) =>
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                MessageBox.Show("Please enter a message");
                return;
            }
            _db.AddFeedback(new Feedback
            {
                TrainerId = _currentUser.Id,
                TrainerName = _currentUser.FullName,
                Message = txtMessage.Text,
                CreatedAt = DateTime.Now
            });
            MessageBox.Show("Feedback sent!");
            txtMessage.Text = "";
        };
        panel.Controls.Add(btnSend);

        return panel;
    }

    private Panel CreateLecturerEnrollTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var lblEnroll = new Label
        {
            Text = "Enroll New Student",
            Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(20, y),
            AutoSize = true
        };
        panel.Controls.Add(lblEnroll);
        y += 40;

        var lblName = new Label { Text = "Name:", Location = new System.Drawing.Point(20, y), Width = 70 };
        panel.Controls.Add(lblName);
        var txtName = new TextBox { Location = new System.Drawing.Point(95, y), Width = 200 };
        panel.Controls.Add(txtName);

        var lblTP = new Label { Text = "TP Number:", Location = new System.Drawing.Point(310, y), Width = 90 };
        panel.Controls.Add(lblTP);
        var txtTP = new TextBox { Location = new System.Drawing.Point(405, y), Width = 130 };
        panel.Controls.Add(txtTP);

        var lblEmail = new Label { Text = "Email:", Location = new System.Drawing.Point(550, y), Width = 60 };
        panel.Controls.Add(lblEmail);
        var txtEmail = new TextBox { Location = new System.Drawing.Point(615, y), Width = 200 };
        panel.Controls.Add(txtEmail);
        y += 35;

        var lblContact = new Label { Text = "Contact:", Location = new System.Drawing.Point(20, y), Width = 70 };
        panel.Controls.Add(lblContact);
        var txtContact = new TextBox { Location = new System.Drawing.Point(95, y), Width = 200 };
        panel.Controls.Add(txtContact);

        var lblAddress = new Label { Text = "Address:", Location = new System.Drawing.Point(310, y), Width = 80 };
        panel.Controls.Add(lblAddress);
        var txtAddress = new TextBox { Location = new System.Drawing.Point(395, y), Width = 420 };
        panel.Controls.Add(txtAddress);
        y += 35;

        var lblModule = new Label { Text = "Module:", Location = new System.Drawing.Point(20, y), Width = 70 };
        panel.Controls.Add(lblModule);
        var cmbModule = new ComboBox { Location = new System.Drawing.Point(95, y), Width = 280 };
        var classes = _db.GetClasses();
        foreach (var c in classes) cmbModule.Items.Add(c.ModuleName + " (" + c.Level + ") - RM" + c.Charges);
        if (classes.Count > 0) cmbModule.SelectedIndex = 0;
        panel.Controls.Add(cmbModule);

        var lblMonth = new Label { Text = "Month:", Location = new System.Drawing.Point(390, y), Width = 70 };
        panel.Controls.Add(lblMonth);
        var cmbMonth = new ComboBox { Location = new System.Drawing.Point(465, y), Width = 150 };
        string[] months = { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
        foreach (var m in months) cmbMonth.Items.Add(m);
        cmbMonth.SelectedIndex = DateTime.Now.Month - 1;
        panel.Controls.Add(cmbMonth);
        y += 40;

        var btnEnroll = new Button { Text = "Enroll Student", Location = new System.Drawing.Point(95, y), Width = 150, Height = 40 };
        btnEnroll.Click += (s, e) =>
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(txtTP.Text) || classes.Count == 0)
            {
                MessageBox.Show("Name, TP Number and available class required");
                return;
            }
            if (cmbModule.SelectedIndex < 0 || cmbModule.SelectedIndex >= classes.Count)
            {
                MessageBox.Show("Please select a valid class");
                return;
            }

            var selectedClass = classes[cmbModule.SelectedIndex];
            _db.AddEnrollment(new Enrollment
            {
                StudentId = 0,
                StudentName = txtName.Text,
                TPNumber = txtTP.Text,
                ClassId = selectedClass.Id,
                ModuleName = selectedClass.ModuleName,
                Level = selectedClass.Level,
                EnrolledMonth = months[cmbMonth.SelectedIndex] + " " + DateTime.Now.Year,
                IsPaid = false,
                IsCompleted = false
            });
            MessageBox.Show("Student enrolled!");
            RefreshDashboard();
        };
        panel.Controls.Add(btnEnroll);

        return panel;
    }

    private Panel CreateLecturerRequestsTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var lblTitle = new Label
        {
            Text = "Student Requests for Extra Coaching",
            Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(20, y),
            AutoSize = true
        };
        panel.Controls.Add(lblTitle);
        y += 40;

        var dgvRequests = new DataGridView
        {
            Location = new System.Drawing.Point(20, y),
            Width = 1050,
            Height = 350,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        dgvRequests.Columns.Add("ID", "ID");
        dgvRequests.Columns.Add("Student", "Student");
        dgvRequests.Columns.Add("Module", "Module");
        dgvRequests.Columns.Add("Level", "Level");
        dgvRequests.Columns.Add("Date", "Date");

        var requests = _db.GetPendingRequests();
        foreach (var r in requests)
        {
            dgvRequests.Rows.Add(r.Id, r.StudentName ?? "", r.ModuleName ?? "", r.Level ?? "", r.CreatedAt.ToString("yyyy-MM-dd"));
        }
        panel.Controls.Add(dgvRequests);
        y += 370;

        var btnApprove = new Button { Text = "Approve", Location = new System.Drawing.Point(20, y), Width = 120, Height = 35 };
        btnApprove.Click += (s, e) =>
        {
            if (dgvRequests.SelectedRows.Count > 0)
            {
                var id = Convert.ToInt32(dgvRequests.SelectedRows[0].Cells[0].Value);
                _db.UpdateRequestStatus(id, "Approved");
                RefreshDashboard();
            }
        };
        panel.Controls.Add(btnApprove);

        var btnReject = new Button { Text = "Reject", Location = new System.Drawing.Point(150, y), Width = 120, Height = 35 };
        btnReject.Click += (s, e) =>
        {
            if (dgvRequests.SelectedRows.Count > 0)
            {
                var id = Convert.ToInt32(dgvRequests.SelectedRows[0].Cells[0].Value);
                _db.UpdateRequestStatus(id, "Rejected");
                RefreshDashboard();
            }
        };
        panel.Controls.Add(btnReject);

        return panel;
    }

    private Panel CreateLecturerStudentsTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var lblTitle = new Label
        {
            Text = "Enrolled Students",
            Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(20, y),
            AutoSize = true
        };
        panel.Controls.Add(lblTitle);
        y += 40;

        var lblLevel = new Label { Text = "Level:", Location = new System.Drawing.Point(20, y), Width = 60 };
        panel.Controls.Add(lblLevel);
        var cmbLevel = new ComboBox { Location = new System.Drawing.Point(85, y), Width = 150 };
        cmbLevel.Items.Add("All Levels");
        cmbLevel.Items.AddRange(new[] { "Beginner", "Intermediate", "Advance" });
        cmbLevel.SelectedIndex = 0;
        panel.Controls.Add(cmbLevel);

        var lblModule = new Label { Text = "Module:", Location = new System.Drawing.Point(250, y), Width = 70 };
        panel.Controls.Add(lblModule);
        var cmbModule = new ComboBox { Location = new System.Drawing.Point(325, y), Width = 200 };
        cmbModule.Items.Add("All Modules");
        foreach (var c in _db.GetClasses()) cmbModule.Items.Add(c.ModuleName);
        cmbModule.SelectedIndex = 0;
        panel.Controls.Add(cmbModule);
        y += 35;

        var dgvStudents = new DataGridView
        {
            Location = new System.Drawing.Point(20, y),
            Width = 1050,
            Height = 320,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        dgvStudents.Columns.Add("ID", "ID");
        dgvStudents.Columns.Add("Name", "Name");
        dgvStudents.Columns.Add("TP Number", "TP Number");
        dgvStudents.Columns.Add("Module", "Module");
        dgvStudents.Columns.Add("Level", "Level");
        dgvStudents.Columns.Add("Paid", "Paid");
        dgvStudents.Columns.Add("Completed", "Completed");
        panel.Controls.Add(dgvStudents);
        y += 340;

        Action refreshList = () =>
        {
            dgvStudents.Rows.Clear();
            var enrollments = _db.GetAllEnrollments();
            foreach (var e in enrollments)
            {
                bool add = true;
                if (cmbLevel.SelectedIndex > 0)
                {
                    var levels = new[] { "", "Beginner", "Intermediate", "Advance" };
                    if (e.Level != levels[cmbLevel.SelectedIndex]) add = false;
                }
                if (cmbModule.SelectedIndex > 0)
                {
                    string selectedModule = cmbModule.Items[cmbModule.SelectedIndex]?.ToString() ?? "";
                    if (e.ModuleName != selectedModule) add = false;
                }

                if (add)
                {
                    string paid = e.IsPaid ? "Yes" : "No";
                    string completed = e.IsCompleted ? "Yes" : "No";
                    dgvStudents.Rows.Add(e.Id, e.StudentName ?? "", e.TPNumber ?? "", e.ModuleName ?? "", e.Level ?? "", paid, completed);
                }
            }
        };
        refreshList();

        cmbLevel.SelectedIndexChanged += (s, e) => refreshList();
        cmbModule.SelectedIndexChanged += (s, e) => refreshList();

        var btnDelete = new Button { Text = "Delete Selected Student", Location = new System.Drawing.Point(20, y), Width = 170, Height = 35 };
        btnDelete.Click += (s, e) =>
        {
            if (dgvStudents.SelectedRows.Count > 0)
            {
                var id = Convert.ToInt32(dgvStudents.SelectedRows[0].Cells[0].Value);
                _db.DeleteEnrollment(id);
                refreshList();
            }
        };
        panel.Controls.Add(btnDelete);

        return panel;
    }

    private Panel CreateStudentScheduleTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var lblTitle = new Label
        {
            Text = "My Class Schedule",
            Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(20, y),
            AutoSize = true
        };
        panel.Controls.Add(lblTitle);
        y += 40;

        var dgvSchedule = new DataGridView
        {
            Location = new System.Drawing.Point(20, y),
            Width = 1050,
            Height = 350,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true
        };
        dgvSchedule.Columns.Add("Module", "Module");
        dgvSchedule.Columns.Add("Level", "Level");
        dgvSchedule.Columns.Add("Schedule", "Schedule");
        dgvSchedule.Columns.Add("Month", "Month");
        dgvSchedule.Columns.Add("Paid", "Paid");

        var enrollments = _db.GetEnrollmentsForStudent(_currentUser.Id);
        foreach (var e in enrollments)
        {
            var cls = _db.GetClassById(e.ClassId);
            string schedule = cls?.Schedule ?? "TBD";
            string paid = e.IsPaid ? "Yes" : "No";
            dgvSchedule.Rows.Add(e.ModuleName ?? "", e.Level ?? "", schedule, e.EnrolledMonth ?? "", paid);
        }
        panel.Controls.Add(dgvSchedule);

        return panel;
    }

    private Panel CreateStudentRequestsTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var lblTitle = new Label
        {
            Text = "Request Additional Coaching",
            Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(20, y),
            AutoSize = true
        };
        panel.Controls.Add(lblTitle);
        y += 40;

        var lblModule = new Label { Text = "Select Module:", Location = new System.Drawing.Point(20, y), Width = 120 };
        panel.Controls.Add(lblModule);

        var cmbModule = new ComboBox { Location = new System.Drawing.Point(145, y), Width = 280 };
        var classes = _db.GetClasses();
        foreach (var c in classes) cmbModule.Items.Add(c.ModuleName + " (" + c.Level + ")");
        if (classes.Count > 0) cmbModule.SelectedIndex = 0;
        panel.Controls.Add(cmbModule);
        y += 35;

        var btnRequest = new Button { Text = "Request Coaching", Location = new System.Drawing.Point(145, y), Width = 150, Height = 35 };
        btnRequest.Click += (s, e) =>
        {
            if (classes.Count == 0)
            {
                MessageBox.Show("No classes available");
                return;
            }
            if (cmbModule.SelectedIndex < 0 || cmbModule.SelectedIndex >= classes.Count)
            {
                MessageBox.Show("Please select a valid class");
                return;
            }
            var selectedClass = classes[cmbModule.SelectedIndex];
            _db.AddRequest(new StudentRequest
            {
                StudentId = _currentUser.Id,
                StudentName = _currentUser.FullName,
                ModuleName = selectedClass.ModuleName,
                Level = selectedClass.Level,
                Status = "Pending",
                CreatedAt = DateTime.Now
            });
            MessageBox.Show("Request submitted!");
            RefreshDashboard();
        };
        panel.Controls.Add(btnRequest);
        y += 50;

        var lblMyRequests = new Label { Text = "My Requests:", Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold), Location = new System.Drawing.Point(20, y), AutoSize = true };
        panel.Controls.Add(lblMyRequests);
        y += 35;

        var dgvRequests = new DataGridView
        {
            Location = new System.Drawing.Point(20, y),
            Width = 1050,
            Height = 250,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        dgvRequests.Columns.Add("ID", "ID");
        dgvRequests.Columns.Add("Module", "Module");
        dgvRequests.Columns.Add("Level", "Level");
        dgvRequests.Columns.Add("Status", "Status");
        dgvRequests.Columns.Add("Date", "Date");

        var requests = _db.GetRequestsForStudent(_currentUser.Id);
        foreach (var r in requests)
        {
            dgvRequests.Rows.Add(r.Id, r.ModuleName ?? "", r.Level ?? "", r.Status ?? "", r.CreatedAt.ToString("yyyy-MM-dd"));
        }
        panel.Controls.Add(dgvRequests);
        y += 265;

        var btnCancel = new Button { Text = "Cancel Pending Request", Location = new System.Drawing.Point(20, y), Width = 170, Height = 35 };
        btnCancel.Click += (s, e) =>
        {
            if (dgvRequests.SelectedRows.Count > 0)
            {
                var id = Convert.ToInt32(dgvRequests.SelectedRows[0].Cells[0].Value);
                var status = dgvRequests.SelectedRows[0].Cells[3].Value?.ToString();
                if (status == "Pending")
                {
                    _db.DeleteRequest(id);
                    RefreshDashboard();
                }
                else
                {
                    MessageBox.Show("Can only cancel pending requests");
                }
            }
        };
        panel.Controls.Add(btnCancel);

        return panel;
    }

    private Panel CreateStudentPaymentTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(15) };
        int y = 20;

        var lblTitle = new Label
        {
            Text = "Invoice View",
            Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(20, y),
            AutoSize = true
        };
        panel.Controls.Add(lblTitle);
        y += 40;

        var lblEnrolled = new Label { Text = "Your Enrolled Modules:", Location = new System.Drawing.Point(20, y) };
        panel.Controls.Add(lblEnrolled);
        y += 30;

        var dgvPayments = new DataGridView
        {
            Location = new System.Drawing.Point(20, y),
            Width = 1050,
            Height = 280,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ReadOnly = true,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect
        };
        dgvPayments.Columns.Add("Module", "Module");
        dgvPayments.Columns.Add("Level", "Level");
        dgvPayments.Columns.Add("Fee", "Fee");
        dgvPayments.Columns.Add("Status", "Status");

        var enrollments = _db.GetEnrollmentsForStudent(_currentUser.Id);
        foreach (var e in enrollments)
        {
            var cls = _db.GetClassById(e.ClassId);
            decimal fee = cls?.Charges ?? 0;
            string paid = e.IsPaid ? "Paid" : "Unpaid";
            dgvPayments.Rows.Add(e.ModuleName ?? "", e.Level ?? "", "RM " + fee.ToString("F2"), paid);
        }
        panel.Controls.Add(dgvPayments);
        y += 300;

        var btnPay = new Button { Text = "Pay for Selected", Location = new System.Drawing.Point(20, y), Width = 150, Height = 40 };
        btnPay.Click += (s, e) =>
        {
            if (dgvPayments.SelectedRows.Count > 0)
            {
                var paidStatus = dgvPayments.SelectedRows[0].Cells[3].Value?.ToString();
                if (paidStatus == "Unpaid")
                {
                    var moduleName = dgvPayments.SelectedRows[0].Cells[0].Value?.ToString();
                    var allClasses = _db.GetClasses();
                    ClassModel? cls = null;
                    for (int i = 0; i < allClasses.Count; i++)
                    {
                        if (allClasses[i].ModuleName == moduleName)
                        {
                            cls = allClasses[i];
                            break;
                        }
                    }
                    if (cls != null)
                    {
                        var enrollment = _db.GetEnrollmentForStudent(_currentUser.Id, cls.Id);
                        if (enrollment != null)
                        {
                            _db.AddPayment(new Payment
                            {
                                EnrollmentId = enrollment.Id,
                                StudentId = _currentUser.Id,
                                Amount = cls.Charges,
                                PaymentDate = DateTime.Now,
                                Status = "Paid"
                            });
                            _db.MarkEnrollmentPaid(enrollment.Id);
                            MessageBox.Show("Payment of RM " + cls.Charges + " successful!");
                            RefreshDashboard();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Already paid");
                }
            }
        };
        panel.Controls.Add(btnPay);
        y += 50;

        var lblFees = new Label { Text = "Note: Fees vary by level - Beginner: RM200, Intermediate: RM300, Advance: RM400", Location = new System.Drawing.Point(20, y), ForeColor = System.Drawing.Color.Gray };
        panel.Controls.Add(lblFees);

        return panel;
    }

    private Panel CreateProfileTab()
    {
        var panel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(20) };
        int y = 30;

        var lblTitle = new Label
        {
            Text = "Profile Settings",
            Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold),
            Location = new System.Drawing.Point(30, y),
            AutoSize = true
        };
        panel.Controls.Add(lblTitle);
        y += 50;

        var lblInfo = new Label
        {
            Text = "Logged in as: " + _currentUser.Role + " - " + _currentUser.Username,
            Location = new System.Drawing.Point(30, y),
            AutoSize = true
        };
        panel.Controls.Add(lblInfo);
        y += 40;

        var lblName = new Label { Text = "Full Name:", Location = new System.Drawing.Point(30, y), Width = 100 };
        panel.Controls.Add(lblName);
        var txtName = new TextBox { Location = new System.Drawing.Point(135, y), Width = 300 };
        txtName.Text = _currentUser.FullName;
        panel.Controls.Add(txtName);
        y += 40;

        var lblEmail = new Label { Text = "Email:", Location = new System.Drawing.Point(30, y), Width = 100 };
        panel.Controls.Add(lblEmail);
        var txtEmail = new TextBox { Location = new System.Drawing.Point(135, y), Width = 300 };
        txtEmail.Text = _currentUser.Email ?? "";
        panel.Controls.Add(txtEmail);
        y += 40;

        var lblContact = new Label { Text = "Contact:", Location = new System.Drawing.Point(30, y), Width = 100 };
        panel.Controls.Add(lblContact);
        var txtContact = new TextBox { Location = new System.Drawing.Point(135, y), Width = 300 };
        txtContact.Text = _currentUser.Contact ?? "";
        panel.Controls.Add(txtContact);
        y += 40;

        var lblPassword = new Label { Text = "New Password:", Location = new System.Drawing.Point(30, y), Width = 100 };
        panel.Controls.Add(lblPassword);
        var txtPassword = new TextBox { Location = new System.Drawing.Point(135, y), Width = 300, PasswordChar = '*' };
        panel.Controls.Add(txtPassword);
        y += 50;

        var btnUpdate = new Button { Text = "Update Profile", Location = new System.Drawing.Point(135, y), Width = 140, Height = 40 };
        btnUpdate.Click += (s, e) =>
        {
            _currentUser.FullName = txtName.Text;
            _currentUser.Email = txtEmail.Text;
            _currentUser.Contact = txtContact.Text;
            _db.UpdateUser(_currentUser);

            if (!string.IsNullOrEmpty(txtPassword.Text))
            {
                _db.UpdatePassword(_currentUser.Id, txtPassword.Text);
            }

            MessageBox.Show("Profile updated!");
        };
        panel.Controls.Add(btnUpdate);

        var btnLogout = new Button { Text = "Logout", Location = new System.Drawing.Point(290, y), Width = 120, Height = 40 };
        btnLogout.Click += (s, e) =>
        {
            this.Close();
        };
        panel.Controls.Add(btnLogout);

        return panel;
    }

    private void RefreshDashboard()
    {
        var selectedTab = tabControl.SelectedIndex;
        LoadDashboard();
        if (selectedTab < tabControl.TabPages.Count)
            tabControl.SelectedIndex = selectedTab;
    }
}