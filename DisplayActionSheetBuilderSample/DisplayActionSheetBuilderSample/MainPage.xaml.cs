using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using static System.Collections.Specialized.BitVector32;

namespace DisplayActionSheetBuilderSample
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        async void Button_Clicked(System.Object sender, System.EventArgs e)
        {
            await new DisplayActionSheetDirector(new DisplayActionSheetFuncBuilder(this)).ShowAsync();
        }
    }

    public interface IBuilder
    {
        IBuilder AddSelectionButton(string title, Action<Page> action);
        IBuilder SetCancelButton(string title, Action<Page> action);
        Func<Task> Create();
    }

    public class DisplayActionSheetFuncBuilder : IBuilder
    {
        Page _page;
        Dictionary<string, Action<Page>> buffer;
        KeyValuePair<string, Action<Page>> _cancelBuffer;


        public DisplayActionSheetFuncBuilder(Page page)
        {
            // 引数でvmを受け取る想定

            _page = page;
            buffer = new Dictionary<string, Action<Page>>();
            _cancelBuffer = new KeyValuePair<string, Action<Page>>("閉じる", (p) => { });
        }

        public IBuilder AddSelectionButton(string title, Action<Page> action)
        {
            buffer[title] = action;
            return this;
        }

        public IBuilder SetCancelButton(string title, Action<Page> action)
        {
            _cancelBuffer = new KeyValuePair<string, Action<Page>>(title, action);
            return this;
        }

        public Func<Task> Create()
        {
            Func<Task> f = async () =>
            {
                var resp = await _page.DisplayActionSheet(null, _cancelBuffer.Key, null, GetButtons());
                if (resp == _cancelBuffer.Key)
                {
                    _cancelBuffer.Value(_page);
                    return;
                }

                buffer.First(x => x.Key == resp).Value(_page);
            };

            return f;
        }

        string[] GetButtons() => buffer.Keys.ToArray();
    }

    public class DisplayActionSheetDirector
    {
        protected IBuilder _builder;

        public DisplayActionSheetDirector(IBuilder builder)
        {
            // 処理にvmが必要な場合はコンストラクタで受け取る

            _builder = builder;
        }

        public async Task ShowAsync()
        {
            _builder.AddSelectionButton("編集", (p) =>
            {
                p.Navigation.PushAsync(new EditPage());
                Console.WriteLine("edit");
            });

            _builder.AddSelectionButton("削除", (p) =>
            {
                Console.WriteLine("delete");
            });

            var func = _builder.Create();
            await func.Invoke();
        }
    }

}

