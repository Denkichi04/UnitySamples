using System;
using App.AudioDemo;
using App.Common;
using App.SampleInGame;
using App.SampleInGame.Domain;
using App.SampleInGame.View;
using App.Title;
using AppCore.Runtime;
using AudioService.Simple;
using Cysharp.Threading.Tasks;
using InputSystemWrapper;
using MessagePipe;
using UnityEngine;

namespace App.Director
{
    public class MainSceneDirector : MonoBehaviour, IDirector
    {
        [SerializeField] private Camera uiCamera;
        
        private TickablePresenter tickablePresenter;

        void Start()
        {
            InitializeAsync().Forget();
        }

        void Update()
        {
            tickablePresenter?.Tick();
        }

        public async Awaitable PushAsync(string key)
        {
            // fadeViewを作成して、フェードインを実行
            var fadeView = await FadeScreenView.CreateAsync();
            fadeView.Push();
            fadeView.Open();
            await fadeView.FadeInAsync();

            // フェードアウトを実行し、FadeViewを破棄
            await fadeView.FadeOutAsync();
            fadeView.Pop();
        }
        
        /// <summary>
        /// 初期化を行う
        /// </summary>
        private async UniTaskVoid InitializeAsync()
        {
            // 画面を真っ暗にする
            var fadeView = await FadeScreenView.CreateAsync();
            fadeView.Push();
            fadeView.Open();
            fadeView.Blackout();

            // オーディオファイルの読み込みを行う
            var audioService = ServiceLocator.Get<SimpleAudioService>();
            await audioService.LoadAllAsyncIfNeed();
            audioService.PlayBgm("BGM_Sample");

            // フェードアウトして、画面表示
            await fadeView.FadeOutAsync();
            fadeView.Pop();
        }
    }
}