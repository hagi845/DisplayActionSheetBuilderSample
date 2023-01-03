using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

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
            const string EDIT_CAPTION = "編集する";
            const string DELETE_CAPTION = "削除する";
            const string CANCEL_CAPTION = "閉じる";

            var buttons = new string[] { EDIT_CAPTION, DELETE_CAPTION };

            var selectedText = await this.DisplayActionSheet(null, CANCEL_CAPTION, null, buttons);

            if (selectedText == CANCEL_CAPTION) return;

            if (selectedText == EDIT_CAPTION)
            {

                await this.DisplayAlert("移動", "コメント編集 のページへ移動します。(未実装)", "OK");
                return;
            }

            if (selectedText == DELETE_CAPTION)
            {
                var result = await this.DisplayAlert("コメントを削除", "本当にこのコメントを削除してよろしいですか？", "削除", "キャンセル");
                if (!result) return;


                return;
            }
        }
    }
}

