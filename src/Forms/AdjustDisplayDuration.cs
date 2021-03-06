﻿using Nikse.SubtitleEdit.Core;
using Nikse.SubtitleEdit.Logic;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace Nikse.SubtitleEdit.Forms
{
    public sealed partial class AdjustDisplayDuration : PositionAndSizeForm
    {
        private const string Sec = "seconds";
        private const string Per = "percent";
        private const string Recal = "recalc";

        public string AdjustValue
        {
            get
            {
                if (radioButtonPercent.Checked)
                    return numericUpDownPercent.Value.ToString(CultureInfo.InvariantCulture);
                if (radioButtonAutoRecalculate.Checked)
                    return $"{radioButtonAutoRecalculate.Text}, {labelMaxCharsPerSecond.Text}: {numericUpDownMaxCharsSec.Value}";
                return numericUpDownSeconds.Value.ToString(CultureInfo.InvariantCulture);
            }
        }

        public bool AdjustUsingPercent => radioButtonPercent.Checked;

        public bool AdjustUsingSeconds => radioButtonSeconds.Checked;

        public decimal MaxCharactersPerSecond => numericUpDownMaxCharsSec.Value;

        public AdjustDisplayDuration()
        {
            UiUtil.PreInitialize(this);
            InitializeComponent();
            UiUtil.FixFonts(this);
            Icon = Properties.Resources.SubtitleEditFormIcon;

            decimal adjustSeconds = Configuration.Settings.Tools.AdjustDurationSeconds;
            if (adjustSeconds >= numericUpDownSeconds.Minimum && adjustSeconds <= numericUpDownSeconds.Maximum)
                numericUpDownSeconds.Value = adjustSeconds;

            int adjustPercent = Configuration.Settings.Tools.AdjustDurationPercent;
            if (adjustPercent >= numericUpDownPercent.Minimum && adjustPercent <= numericUpDownPercent.Maximum)
                numericUpDownPercent.Value = adjustPercent;

            numericUpDownMaxCharsSec.Value = (decimal)Configuration.Settings.General.SubtitleMaximumCharactersPerSeconds;

            LanguageStructure.AdjustDisplayDuration language = Configuration.Settings.Language.AdjustDisplayDuration;
            Text = language.Title;
            groupBoxAdjustVia.Text = language.AdjustVia;
            radioButtonSeconds.Text = language.Seconds;
            radioButtonPercent.Text = language.Percent;
            radioButtonAutoRecalculate.Text = language.Recalculate;
            labelMaxCharsPerSecond.Text = Configuration.Settings.Language.Settings.MaximumCharactersPerSecond;
            labelAddSeconds.Text = language.AddSeconds;
            labelAddInPercent.Text = language.SetAsPercent;
            labelNote.Text = language.Note;
            buttonOK.Text = Configuration.Settings.Language.General.Ok;
            buttonCancel.Text = Configuration.Settings.Language.General.Cancel;
            FixLargeFonts();

            switch (Configuration.Settings.Tools.AdjustDurationLast)
            {
                case Sec:
                    radioButtonSeconds.Checked = true;
                    break;
                case Per:
                    radioButtonPercent.Checked = true;
                    break;
                case Recal:
                    radioButtonAutoRecalculate.Checked = true;
                    break;
            }
        }

        private void FixLargeFonts()
        {
            if (labelNote.Left + labelNote.Width + 5 > Width)
                Width = labelNote.Left + labelNote.Width + 5;
            UiUtil.FixLargeFonts(this, buttonOK);
        }

        private void FormAdjustDisplayTime_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                DialogResult = DialogResult.Cancel;
        }

        private void RadioButtonPercentCheckedChanged(object sender, EventArgs e)
        {
            FixEnabled();
        }

        private void FixEnabled()
        {
            numericUpDownPercent.Enabled = radioButtonPercent.Checked;
            numericUpDownSeconds.Enabled = radioButtonSeconds.Checked;
            numericUpDownMaxCharsSec.Enabled = radioButtonAutoRecalculate.Checked;
        }

        private void RadioButtonSecondsCheckedChanged(object sender, EventArgs e)
        {
            FixEnabled();
        }

        private void ButtonOkClick(object sender, EventArgs e)
        {
            Configuration.Settings.Tools.AdjustDurationSeconds = numericUpDownSeconds.Value;
            Configuration.Settings.Tools.AdjustDurationPercent = (int)numericUpDownPercent.Value;

            if (radioButtonSeconds.Checked)
            {
                Configuration.Settings.Tools.AdjustDurationLast = Sec;
            }
            else if (radioButtonPercent.Checked)
            {
                Configuration.Settings.Tools.AdjustDurationLast = Per;
            }
            else if (radioButtonAutoRecalculate.Checked)
            {
                Configuration.Settings.Tools.AdjustDurationLast = Recal;
            }

            DialogResult = DialogResult.OK;
        }

        public void HideRecalculate()
        {
            if (radioButtonAutoRecalculate.Checked)
                radioButtonSeconds.Checked = true;
            radioButtonAutoRecalculate.Visible = false;
            labelMaxCharsPerSecond.Visible = false;
            numericUpDownMaxCharsSec.Visible = false;
        }

    }
}
