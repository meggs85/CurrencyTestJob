namespace CurrencyScanner
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.btLoadPrice = new System.Windows.Forms.Button();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.calend = new System.Windows.Forms.MonthCalendar();
            this.label1 = new System.Windows.Forms.Label();
            this.lbTickers = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // btLoadPrice
            // 
            this.btLoadPrice.Location = new System.Drawing.Point(404, 13);
            this.btLoadPrice.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btLoadPrice.Name = "btLoadPrice";
            this.btLoadPrice.Size = new System.Drawing.Size(139, 36);
            this.btLoadPrice.TabIndex = 0;
            this.btLoadPrice.Text = "Загрузить";
            this.btLoadPrice.UseVisualStyleBackColor = true;
            this.btLoadPrice.Click += new System.EventHandler(this.btLoadPrice_Click);
            // 
            // lbLog
            // 
            this.lbLog.FormattingEnabled = true;
            this.lbLog.HorizontalScrollbar = true;
            this.lbLog.ItemHeight = 16;
            this.lbLog.Location = new System.Drawing.Point(12, 242);
            this.lbLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(772, 196);
            this.lbLog.TabIndex = 1;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // timer
            // 
            this.timer.Enabled = true;
            this.timer.Interval = 3600000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // calend
            // 
            this.calend.Location = new System.Drawing.Point(13, 13);
            this.calend.Name = "calend";
            this.calend.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(401, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "lbPrice";
            // 
            // lbTickers
            // 
            this.lbTickers.AllowDrop = true;
            this.lbTickers.FormattingEnabled = true;
            this.lbTickers.ItemHeight = 16;
            this.lbTickers.Location = new System.Drawing.Point(265, 13);
            this.lbTickers.Name = "lbTickers";
            this.lbTickers.Size = new System.Drawing.Size(120, 196);
            this.lbTickers.Sorted = true;
            this.lbTickers.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(831, 450);
            this.Controls.Add(this.lbTickers);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.calend);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.btLoadPrice);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btLoadPrice;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.ListBox lbTickers;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MonthCalendar calend;
    }
}

