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

                    lblRoleSize.Text = asmVirtualMachine.RoleSize;
                    lblOS.Text = asmVirtualMachine.OSVirtualHardDiskOS;
                }
                else if (_VirtualMachine.Source.GetType() == typeof(Azure.Arm.VirtualMachine))
                {
                    Azure.Arm.VirtualMachine armVirtualMachine = (Azure.Arm.VirtualMachine)_VirtualMachine.Source;

                    lblRoleSize.Text = armVirtualMachine.VmSize;
                    lblOS.Text = armVirtualMachine.OSVirtualHardDiskOS;
                }
            }

            if (_VirtualMachine.OSVirtualHardDisk != null)
                this.diskProperties1.Bind(azureContext, _TargetTreeView, _VirtualMachine.OSVirtualHardDisk);
            else
                this.diskProperties1.Visible = false;
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
    }
}
