using System.Windows.Forms;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.ReportDesigner.Properties;
using InfinniPlatform.ReportDesigner.Views.Wizard;

namespace InfinniPlatform.ReportDesigner.Views.DataSources
{
    /// <summary>
    ///     Мастер для создания источника данных.
    /// </summary>
    internal sealed class DataSourceWizard
    {
        private readonly DialogView<SqlDataSourceView> _firebirdDataSourceDialog;
        private readonly SqlDataSourceView _firebirdDataSourceView;
        private readonly DialogView<SqlTableSelectView> _firebirdTableSelectDialog;
        private readonly SqlTableSelectView _firebirdTableSelectView;
        private readonly DialogView<SqlDataSourceView> _msSqlDataSourceDialog;
        private readonly SqlDataSourceView _msSqlDataSourceView;
        private readonly DialogView<SqlTableSelectView> _msSqlTableSelectDialog;
        private readonly SqlTableSelectView _msSqlTableSelectView;
        private readonly DataSourceProviderSelectView _providerSelectView;
        private readonly DialogView<RegisterDataSourceView> _registerDataSourceDialog;
        private readonly RegisterDataSourceView _registerDataSourceView;
        private readonly DialogView<RestDataSourceView> _restDataSourceDialog;
        private readonly RestDataSourceView _restDataSourceView;
        private readonly WizardView _wizardView;

        public DataSourceWizard()
        {
            _wizardView = new WizardView {Text = Resources.DataSourceWizard};


            // Представления выбора провайдера
            _providerSelectView = new DataSourceProviderSelectView();

            // Представления для Register
            _registerDataSourceView = new RegisterDataSourceView();
            _registerDataSourceDialog = new DialogView<RegisterDataSourceView>(_registerDataSourceView);

            // Представления для REST
            _restDataSourceView = new RestDataSourceView();
            _restDataSourceDialog = new DialogView<RestDataSourceView>(_restDataSourceView);

            // Представления для MSSQL
            _msSqlTableSelectView = new SqlTableSelectView(new MsSqlMetadataProvider());
            _msSqlTableSelectDialog = new DialogView<SqlTableSelectView>(_msSqlTableSelectView);
            _msSqlDataSourceView = new SqlDataSourceView();
            _msSqlDataSourceDialog = new DialogView<SqlDataSourceView>(_msSqlDataSourceView);
            _msSqlDataSourceView.DataSchemaSelectDialog = _msSqlTableSelectDialog;

            // Представления для Firebird
            _firebirdTableSelectView = new SqlTableSelectView(new FirebirdMetadataProvider());
            _firebirdTableSelectDialog = new DialogView<SqlTableSelectView>(_firebirdTableSelectView);
            _firebirdDataSourceView = new SqlDataSourceView();
            _firebirdDataSourceDialog = new DialogView<SqlDataSourceView>(_msSqlDataSourceView);
            _firebirdDataSourceView.DataSchemaSelectDialog = _firebirdTableSelectDialog;


            // Шаг 1: Выбор поставщика данных
            _wizardView.SetupSteps(_providerSelectView, root => root
                .OnNext(_providerSelectView.ValidateChildren)
                .OnReset(_providerSelectView.ResetDefaults)
                // Шаг 2: Если выбран "Register"
                .AddPage(_registerDataSourceView, selectPage => selectPage
                    .Condition(() => _providerSelectView.SelectedProvider == DataSourceProviderType.Register)
                    .OnBack(OnRegisterPageBack)
                    .OnReset(_registerDataSourceView.ResetDefaults)
                    .OnFinish(OnRegisterFinish))
                // Шаг 2: Если выбран "RestService"
                .AddPage(_restDataSourceView, dataSourcePage => dataSourcePage
                    .Condition(() => _providerSelectView.SelectedProvider == DataSourceProviderType.RestService)
                    .OnBack(OnRestPageBack)
                    .OnReset(_restDataSourceView.ResetDefaults)
                    .OnFinish(OnRestFinish))
                // Шаг 2: Если выбран "MsSqlServer"
                .AddPage(_msSqlTableSelectView, selectPage => selectPage
                    .Condition(() => _providerSelectView.SelectedProvider == DataSourceProviderType.MsSqlServer)
                    .OnNext(OnMsSqlTableSelected)
                    // Шаг 3: Настройка источника данных
                    .AddPage(_msSqlDataSourceView, dataSourcePage => dataSourcePage
                        .OnBack(OnMsSqlPageBack)
                        .OnReset(_msSqlDataSourceView.ResetDefaults)
                        .OnFinish(OnMsSqlFinish)))
                // Шаг 2: Если выбран "Firebird"
                .AddPage(_firebirdTableSelectView, selectPage => selectPage
                    .Condition(() => _providerSelectView.SelectedProvider == DataSourceProviderType.Firebird)
                    .OnNext(OnFirebirdTableSelected)
                    // Шаг 3: Настройка источника данных
                    .AddPage(_firebirdDataSourceView, dataSourcePage => dataSourcePage
                        .OnBack(OnFirebirdPageBack)
                        .OnReset(_firebirdDataSourceView.ResetDefaults)
                        .OnFinish(OnFirebirdFinish))));
        }

        // PUBLIC

        public DataSourceInfo DataSourceInfo { get; set; }
        // REGISTER HANDLERS
        private bool OnRegisterPageBack()
        {
            _registerDataSourceView.ResetDefaults();

            return true;
        }

        private bool OnRegisterFinish()
        {
            if (_registerDataSourceView.ValidateChildren())
            {
                DataSourceInfo = _registerDataSourceView.DataSourceInfo;

                return true;
            }

            return false;
        }

        // REST HANDLERS

        private bool OnRestPageBack()
        {
            _restDataSourceView.ResetDefaults();

            return true;
        }

        private bool OnRestFinish()
        {
            if (_restDataSourceView.ValidateChildren())
            {
                DataSourceInfo = _restDataSourceView.DataSourceInfo;

                return true;
            }

            return false;
        }

        // MSSQL HANDLERS

        private bool OnMsSqlTableSelected()
        {
            if (_msSqlTableSelectView.ValidateChildren())
            {
                _msSqlTableSelectView.AsyncLoadDataSourceInfo(_wizardView,
                    dataSourceInfo => _msSqlDataSourceView.DataSourceInfo = dataSourceInfo);

                return true;
            }

            return false;
        }

        private bool OnMsSqlPageBack()
        {
            _msSqlDataSourceView.ResetDefaults();

            return true;
        }

        private bool OnMsSqlFinish()
        {
            if (_msSqlDataSourceView.ValidateChildren())
            {
                DataSourceInfo = _msSqlDataSourceView.DataSourceInfo;

                return true;
            }

            return false;
        }

        // FIREBIRD HANDLERS

        private bool OnFirebirdTableSelected()
        {
            if (_firebirdTableSelectView.ValidateChildren())
            {
                _firebirdTableSelectView.AsyncLoadDataSourceInfo(_wizardView,
                    dataSourceInfo => _firebirdDataSourceView.DataSourceInfo = dataSourceInfo);

                return true;
            }

            return false;
        }

        private bool OnFirebirdPageBack()
        {
            _firebirdDataSourceView.ResetDefaults();

            return true;
        }

        private bool OnFirebirdFinish()
        {
            if (_firebirdDataSourceView.ValidateChildren())
            {
                DataSourceInfo = _firebirdDataSourceView.DataSourceInfo;

                return true;
            }

            return false;
        }

        public DialogResult ShowDialog(IWin32Window owner)
        {
            var result = DialogResult.Cancel;

            if (DataSourceInfo != null)
            {
                if (DataSourceInfo.Provider is RegisterDataProviderInfo)
                {
                    _registerDataSourceView.DataSourceInfo = DataSourceInfo;
                    _registerDataSourceDialog.View = _registerDataSourceView;

                    result = _registerDataSourceDialog.ShowDialog(owner);

                    if (result == DialogResult.OK)
                    {
                        DataSourceInfo = _registerDataSourceView.DataSourceInfo;
                    }
                }
                else if (DataSourceInfo.Provider is RestDataProviderInfo)
                {
                    _restDataSourceView.DataSourceInfo = DataSourceInfo;
                    _restDataSourceDialog.View = _restDataSourceView;

                    result = _restDataSourceDialog.ShowDialog(owner);

                    if (result == DialogResult.OK)
                    {
                        DataSourceInfo = _restDataSourceView.DataSourceInfo;
                    }
                }
                else if (DataSourceInfo.Provider is SqlDataProviderInfo)
                {
                    var sqlProvider = (SqlDataProviderInfo) DataSourceInfo.Provider;

                    if (sqlProvider.ServerType == SqlServerType.MsSqlServer)
                    {
                        _msSqlTableSelectDialog.View = _msSqlTableSelectView;
                        _msSqlDataSourceView.DataSourceInfo = DataSourceInfo;
                        _msSqlDataSourceDialog.View = _msSqlDataSourceView;

                        result = _msSqlDataSourceDialog.ShowDialog(owner);

                        if (result == DialogResult.OK)
                        {
                            DataSourceInfo = _msSqlDataSourceView.DataSourceInfo;
                        }
                    }
                    else if (sqlProvider.ServerType == SqlServerType.Firebird)
                    {
                        _firebirdTableSelectDialog.View = _firebirdTableSelectView;
                        _firebirdDataSourceView.DataSourceInfo = DataSourceInfo;
                        _firebirdDataSourceDialog.View = _firebirdDataSourceView;

                        result = _firebirdDataSourceDialog.ShowDialog(owner);

                        if (result == DialogResult.OK)
                        {
                            DataSourceInfo = _firebirdDataSourceView.DataSourceInfo;
                        }
                    }
                }
            }
            else
            {
                _wizardView.Reset();

                result = _wizardView.ShowDialog(owner);
            }

            return result;
        }
    }
}