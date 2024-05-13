
namespace лаба_3
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.start = new System.Windows.Forms.Button();
            this.stop = new System.Windows.Forms.Button();
            this.stonecount = new System.Windows.Forms.TextBox();
            this.papercount = new System.Windows.Forms.TextBox();
            this.scissorscount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.abort = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.speedbox = new System.Windows.Forms.TextBox();
            this.UserText = new System.Windows.Forms.TextBox();
            this.winComboBox = new System.Windows.Forms.ComboBox();
            this.labelScore = new System.Windows.Forms.Label();
            this.clearScoreButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(148, 77);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(64, 41);
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(13, 13);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(94, 29);
            this.start.TabIndex = 0;
            this.start.Text = "Start";
            this.start.UseVisualStyleBackColor = true;
            this.start.Click += new System.EventHandler(this.button1_Click);
            // 
            // stop
            // 
            this.stop.Location = new System.Drawing.Point(113, 13);
            this.stop.Name = "stop";
            this.stop.Size = new System.Drawing.Size(114, 29);
            this.stop.TabIndex = 1;
            this.stop.Text = "Pause/Resume";
            this.stop.UseVisualStyleBackColor = true;
            this.stop.Click += new System.EventHandler(this.stop_Click);
            // 
            // stonecount
            // 
            this.stonecount.Location = new System.Drawing.Point(403, 15);
            this.stonecount.Name = "stonecount";
            this.stonecount.Size = new System.Drawing.Size(39, 27);
            this.stonecount.TabIndex = 2;
            // 
            // papercount
            // 
            this.papercount.Location = new System.Drawing.Point(514, 14);
            this.papercount.Name = "papercount";
            this.papercount.Size = new System.Drawing.Size(41, 27);
            this.papercount.TabIndex = 3;
            // 
            // scissorscount
            // 
            this.scissorscount.Location = new System.Drawing.Point(642, 15);
            this.scissorscount.Name = "scissorscount";
            this.scissorscount.Size = new System.Drawing.Size(44, 27);
            this.scissorscount.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(350, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 20);
            this.label1.TabIndex = 5;
            this.label1.Text = "Stone";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(462, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Paper";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(576, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 20);
            this.label3.TabIndex = 7;
            this.label3.Text = "Scissors";
            // 
            // abort
            // 
            this.abort.Location = new System.Drawing.Point(232, 13);
            this.abort.Name = "abort";
            this.abort.Size = new System.Drawing.Size(94, 29);
            this.abort.TabIndex = 8;
            this.abort.Text = "Delete";
            this.abort.UseVisualStyleBackColor = true;
            this.abort.Click += new System.EventHandler(this.abort_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(692, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(79, 20);
            this.label4.TabIndex = 9;
            this.label4.Text = "MaxSpeed";
            // 
            // speedbox
            // 
            this.speedbox.Location = new System.Drawing.Point(772, 15);
            this.speedbox.Name = "speedbox";
            this.speedbox.Size = new System.Drawing.Size(44, 27);
            this.speedbox.TabIndex = 10;
            // 
            // UserText
            // 
            this.UserText.Location = new System.Drawing.Point(13, 49);
            this.UserText.Name = "UserText";
            this.UserText.Size = new System.Drawing.Size(94, 27);
            this.UserText.TabIndex = 11;
            // 
            // winComboBox
            // 
            this.winComboBox.FormattingEnabled = true;
            this.winComboBox.Location = new System.Drawing.Point(123, 49);
            this.winComboBox.Name = "winComboBox";
            this.winComboBox.Size = new System.Drawing.Size(104, 28);
            this.winComboBox.TabIndex = 12;
            // 
            // labelScore
            // 
            this.labelScore.AutoSize = true;
            this.labelScore.Location = new System.Drawing.Point(863, 21);
            this.labelScore.Name = "labelScore";
            this.labelScore.Size = new System.Drawing.Size(46, 20);
            this.labelScore.TabIndex = 13;
            this.labelScore.Text = "Score";
            this.labelScore.Click += new System.EventHandler(this.label5_Click);
            // 
            // clearScoreButton
            // 
            this.clearScoreButton.Location = new System.Drawing.Point(245, 49);
            this.clearScoreButton.Name = "clearScoreButton";
            this.clearScoreButton.Size = new System.Drawing.Size(94, 29);
            this.clearScoreButton.TabIndex = 14;
            this.clearScoreButton.Text = "Clear Score";
            this.clearScoreButton.UseVisualStyleBackColor = true;
            this.clearScoreButton.Click += new System.EventHandler(this.clearScoreButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(1016, 516);
            this.Controls.Add(this.clearScoreButton);
            this.Controls.Add(this.labelScore);
            this.Controls.Add(this.winComboBox);
            this.Controls.Add(this.UserText);
            this.Controls.Add(this.speedbox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.abort);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.scissorscount);
            this.Controls.Add(this.papercount);
            this.Controls.Add(this.stonecount);
            this.Controls.Add(this.stop);
            this.Controls.Add(this.start);
            this.Name = "Form1";
            this.Text = "зонби дабалатория v 2.0;";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.Button stop;
        private System.Windows.Forms.TextBox stonecount;
        private System.Windows.Forms.TextBox papercount;
        private System.Windows.Forms.TextBox scissorscount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button abort;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox speedbox;
        private System.Windows.Forms.TextBox UserText;
        private System.Windows.Forms.ComboBox winComboBox;
        private System.Windows.Forms.Label labelScore;
        private System.Windows.Forms.Button clearScoreButton;
    }
}

