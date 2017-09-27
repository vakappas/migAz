using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using MigAz.Core.Interface;

namespace MigAz.Azure.UserControls
{
    public partial class VirtualMachineProperties : UserControl
    {
        private AzureContext _AzureContext;
        private TargetTreeView _TargetTreeView;
        private MigrationTarget.VirtualMachine _VirtualMachine;

        public delegate Task AfterPropertyChanged();
        public event AfterPropertyChanged PropertyChanged;

        public bool AllowManangedDisk
        {
            get { return this.diskProperties1.AllowManangedDisk; }
            set { this.diskProperties1.AllowManangedDisk = value; }
        }

        public VirtualMachineProperties()
        {
            InitializeComponent();
            this.diskProperties1.PropertyChanged += Properties1_PropertyChanged;
            this.diskProperties1.ShowSizeInGb = false;
        }

        public async Task Bind(AzureContext azureContext, TargetTreeView targetTreeView, MigrationTarget.VirtualMachine virtualMachine)
        {
            _AzureContext = azureContext;
            _TargetTreeView = targetTreeView;
            _VirtualMachine = virtualMachine;

            txtTargetName.Text = _VirtualMachine.TargetName;

            if (_VirtualMachine.Source != null)
            {
                if (_VirtualMachine.Source.GetType() == typeof(Azure.Asm.VirtualMachine))
                {
                    Azure.Asm.VirtualMachine asmVirtualMachine = (Azure.Asm.VirtualMachine)_VirtualMachine.Source;

                    if (asmVirtualMachine.RoleSize != null)
                    {
                        lblRoleSize.Text = asmVirtualMachine.RoleSize.Name;
                        lblSourceCPUCores.Text = asmVirtualMachine.RoleSize.Cores.ToString();
                        lblSourceMemoryInGb.Text = ((double)asmVirtualMachine.RoleSize.MemoryInMb / 1024).ToString();
                        lblSourceMaxDataDisks.Text = asmVirtualMachine.RoleSize.MaxDataDiskCount.ToString();
                    }

                    lblOS.Text = asmVirtualMachine.OSVirtualHardDiskOS;
                }
                else if (_VirtualMachine.Source.GetType() == typeof(Azure.Arm.VirtualMachine))
                {
                    Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)_VirtualMachine.Source;

                    if (armVirtualMachine.VmSize != null)
                    {
                        lblRoleSize.Text = armVirtualMachine.VmSize.ToString();
                        lblSourceCPUCores.Text = armVirtualMachine.VmSize.NumberOfCores.ToString();
                        lblSourceMemoryInGb.Text = ((double)armVirtualMachine.VmSize.memoryInMB / 1024).ToString();
                        lblSourceMaxDataDisks.Text = armVirtualMachine.VmSize.maxDataDiskCount.ToString();
                    }

                    lblOS.Text = armVirtualMachine.OSVirtualHardDiskOS;
                }
            }

            if (_VirtualMachine.OSVirtualHardDisk != null)
                this.diskProperties1.Bind(azureContext, _TargetTreeView, _VirtualMachine.OSVirtualHardDisk);
            else
                this.diskProperties1.Visible = false;

            cbRoleSizes.Items.Clear();
            if (targetTreeView.TargetResourceGroup != null && targetTreeView.TargetResourceGroup.TargetLocation != null)
            {
                cbRoleSizes.Enabled = true;
                cbRoleSizes.Visible = true;
                lblTargetLocationRequired.Enabled = false;
                lblTargetLocationRequired.Visible = false;

                if (targetTreeView.TargetResourceGroup.TargetLocation.VMSizes != null)
                {
                    foreach (Arm.VMSize vmSize in targetTreeView.TargetResourceGroup.TargetLocation.VMSizes)
                    {
                        cbRoleSizes.Items.Add(vmSize);
                    }
                }

                if (_VirtualMachine.TargetSize != null)
                {
                    int sizeIndex = cbRoleSizes.FindStringExact(_VirtualMachine.TargetSize.ToString());
                    cbRoleSizes.SelectedIndex = sizeIndex;
                }
            }
            else
            {
                cbRoleSizes.Enabled = false;
                cbRoleSizes.Visible = false;
                lblTargetLocationRequired.Enabled = true;
                lblTargetLocationRequired.Visible = true;
            }
        }

        private async Task Properties1_PropertyChanged()
        {
            await PropertyChanged();
        }

        private void txtTargetName_TextChanged(object sender, EventArgs e)
        {
            _VirtualMachine.TargetName = txtTargetName.Text;

            PropertyChanged();
        }

        private void txtTargetName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsWhiteSpace(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void cbRoleSizes_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbRoleSizes.SelectedItem != null)
            {
                Arm.VMSize selectedSize = (Arm.VMSize)cbRoleSizes.SelectedItem;

                lblTargetNumberOfCores.Text = selectedSize.NumberOfCores.ToString();
                lblTargetMemoryInGb.Text = ((double)selectedSize.memoryInMB / 1024).ToString();
                lblTargetMaxDataDisks.Text = selectedSize.maxDataDiskCount.ToString();

                _VirtualMachine.TargetSize = selectedSize;
            }
            else
                _VirtualMachine.TargetSize = null;

            PropertyChanged();
        }
    }
}
