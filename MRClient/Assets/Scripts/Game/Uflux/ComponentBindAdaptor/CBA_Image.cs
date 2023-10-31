using BDFramework.UFlux;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine.AddressableAssets;
//这里的命名空间必须为：BDFramework.Uflux
namespace BDFramework.UFlux
{
    /// <summary>
    /// 这里是UnityEngine的UI Image适配器
    /// </summary>
    [ComponentBindAdaptor(typeof(Image))]
    public class CBA_Image : AComponentBindAdaptor
    {
        public override void Init()
        {
            base.Init();
            setPropComponentBindMap[nameof(Image.sprite)] = SetProp_Sprite;
            setPropComponentBindMap[nameof(Image.overrideSprite)] = SetProp_OverrideSprite;
            setPropComponentBindMap[nameof(Image.color)] = SetProp_Color;
            setPropComponentBindMap[nameof(Image.fillAmount)] = SetProp_Amount;
        }

        /// <summary>
        /// 设置图片
        /// </summary>
        /// <param name="value"></param>
        private async void SetProp_Sprite(UIBehaviour uiBehaviour, object value)
        {
            var img = uiBehaviour as Image;
            if (value is string)
            {
                CancellationToken token = new CancellationToken();
                var sprite = await Addressables.LoadAssetAsync<Sprite>($"{value}.png")
                   .WithCancellation(token);
                img.sprite = sprite;
            }
            else if (value is Sprite)
            {
                img.sprite = (Sprite)value;
            }
        }

        /// <summary>
        /// 设置图片
        /// </summary>
        /// <param name="value"></param>
        private async void SetProp_OverrideSprite(UIBehaviour uiBehaviour, object value)
        {
            var img = uiBehaviour as Image;
            if (value is string path)
            {
                img.sprite = UFluxUtils.LoadSprite(path);
            }
            else if (value is Sprite sprite)
            {
                img.overrideSprite = sprite;
            }
        }

        /// <summary>
        /// 设置颜色
        /// </summary>
        /// <param name="uiBehaviour"></param>
        /// <param name="value"></param>
        private void SetProp_Color(UIBehaviour uiBehaviour, object value)
        {
            var img = uiBehaviour as Image;
            if (value is Color)
            {
                img.color = (Color)value;
            }
        }

        private void SetProp_Amount(UIBehaviour uiBehaviour, object value)
        {
            var img = uiBehaviour as Image;
            if (value is float)
            {
                img.fillAmount = (float)value;
            }
        }
    }
}