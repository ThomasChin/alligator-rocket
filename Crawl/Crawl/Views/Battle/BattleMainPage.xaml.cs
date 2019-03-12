using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Crawl.Models;
using Crawl.ViewModels;
using Crawl.GameEngine;

namespace Crawl.Views.Battle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BattleMainPage : ContentPage
    {
        // Hold the Selected Characters
        OpeningPage _myModalCharacterSelectPage;

        // Hold the Monsters
        BattleMonsterListPage _myModalBattleMonsterListPage;

        // Hold the Game Over
        BattleOverPage _myModalBattleGameOverPage;

        // HTML Formatting for message output box
        HtmlWebViewSource htmlSource = new HtmlWebViewSource();

        // Score
        Score myScoreObject = new Score();

        // Hold the Veiw Model
        private BattleViewModel _viewModel;

        /// <summary>
        /// Stand up the Page and initiate state
        /// </summary>
        public BattleMainPage()
        {
            _viewModel = BattleViewModel.Instance;
            BindingContext = _viewModel;

            InitializeComponent();

            // Show the Next button, hide the Game Over button
            GameNextButton.IsVisible = true;
            GameOverButton.IsVisible = false;

            _viewModel.StartBattle();
            Debug.WriteLine("Battle Start" + " Characters :" + _viewModel.BattleEngine.CharacterList.Count);

            // Load the Characters into the Battle Engine
            _viewModel.LoadCharacters();

            // Start the Round
            _viewModel.StartRound();
            Debug.WriteLine("Round Start" + " Monsters:" + _viewModel.BattleEngine.MonsterList.Count);

            // Clear the Screen
            ClearMessages();

            ShowModalPageMonsterList();

            AllCharactersToScreen();
        }

        /// <summary>
        ///  Clears the messages on the UX
        /// </summary>
        public void ClearMessages()
        {
            MessageText.Text = "";
            htmlSource.Html = _viewModel.BattleEngine.BattleMessages.GetHTMLBlankMessage();
            HtmlBox.Source = htmlSource;
        }

        /// <summary>
        /// Next Turn Button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public async void OnNextClicked(object sender, EventArgs args)
        {
            // Hold the current state
            var CurrentRoundState = _viewModel.BattleEngine.RoundStateEnum;

            // If the round is over start a new one...
            if (CurrentRoundState == RoundEnum.NewRound)
            {
                _viewModel.NewRound();
                MessagingCenter.Send(this, "NewRound");

                Debug.WriteLine("New Round :" + _viewModel.BattleEngine.BattleScore.RoundCount);

                ShowModalPageMonsterList();
            }

            // Check for Game Over
            if (CurrentRoundState == RoundEnum.GameOver)
            {
                _viewModel.EndBattle();
                MessagingCenter.Send(this, "EndBattle");
                Debug.WriteLine("End Battle");

                // Output Formatted Results 
                var myResult = _viewModel.BattleEngine.GetResultsOutput();
                Debug.Write(myResult);

                // Let the user know the game is over
                ClearMessages();    // Clear message
                AppendMessage("Game Over\n"); // Show Game Over

                // Clear the players from the center of the board
                DrawGameBoardClear();

                // Change to the Game Over Button
                GameNextButton.IsVisible = false;
                GameOverButton.IsVisible = true;


                // Save the Score to the Score View Model, by sending a message to it.
                var myScore = _viewModel.BattleEngine.BattleScore;
                MessagingCenter.Send(this, "AddData", myScore);

                return;
            }

            // Do the turn...
            _viewModel.RoundNextTurn();
            MessagingCenter.Send(this, "RoundNextTurn");

            // Output the Game Board
            DrawGameBoardAttackerDefender();

            // Output The Message that happened.
            GameMessage();
        }

        /// <summary>
        /// Draws the Game Board Attacker and Defender
        /// </summary>
        public void DrawGameBoardAttackerDefender()
        {
            AttackerName.Text = _viewModel.BattleEngine.CurrentAttacker.Name;
            AttackerHealth.Text = _viewModel.BattleEngine.CurrentAttacker.CurrentHealth.ToString() + " / " + _viewModel.BattleEngine.CurrentAttacker.MaxHealth.ToString();

            DefenderName.Text = _viewModel.BattleEngine.CurrentDefender.Name;
            DefenderHealth.Text = _viewModel.BattleEngine.CurrentDefender.CurrentHealth.ToString() + " / " + _viewModel.BattleEngine.CurrentDefender.MaxHealth.ToString();
        }

        /// <summary>
        /// Draws the Game Board Attacker and Defender areas to be null
        /// </summary>
        public void DrawGameBoardClear()
        {
            AttackerName.Text = string.Empty;
            AttackerHealth.Text = string.Empty;

            DefenderName.Text = string.Empty;
            DefenderHealth.Text = string.Empty;

            BattlePlayerBoxVersus.Text = string.Empty;
        }

        /// <summary>
        /// Append new message in front of old message (makes it a stack from top down)
        /// </summary>
        /// <param name="message"></param>
        public void AppendMessage(string message)
        {
            MessageText.Text = message + "\n" + MessageText.Text;
        }

        /// <summary>
        /// Builds up the output message
        /// </summary>
        /// <param name="message"></param>
        public void GameMessage()
        {

            var message = _viewModel.BattleEngine.BattleMessages.TurnMessage;
            var levelMessage = _viewModel.BattleEngine.BattleMessages.LevelUpMessage;
            Debug.WriteLine("Message: " + message);

            AppendMessage(message);
            AppendMessage(levelMessage);

            htmlSource.Html = _viewModel.BattleEngine.BattleMessages.GetHTMLFormattedTurnMessage();
            HtmlBox.Source = HtmlBox.Source = htmlSource;
        }

        /// <summary>
        /// Show the Game Over Screen
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public async void OnGameOverClicked(object sender, EventArgs args)
        {
            var myScore = _viewModel.BattleEngine.BattleScore.ScoreTotal;
            var outputString = "Battle Over! Score " + myScore.ToString();
            Debug.WriteLine(outputString);

            myScoreObject = _viewModel.BattleEngine.BattleScore;
            await Navigation.PushAsync(new ScoreDetailPage(new ScoreDetailViewModel(myScoreObject)));
        }

        // Helper to handle Modal navigation.
        private void HandleModalPopping(object sender, ModalPoppingEventArgs e)
        {
            if (e.Modal == _myModalBattleMonsterListPage)
            {
                _myModalBattleMonsterListPage = null;

                // remember to remove the event handler:
                App.Current.ModalPopping -= HandleModalPopping;
            }

            if (e.Modal == _myModalCharacterSelectPage)
            {
                _myModalCharacterSelectPage = null;

                // remember to remove the event handler:
                App.Current.ModalPopping -= HandleModalPopping;
            }
        }

        // Show Monsters
        private async void ShowModalPageMonsterList()
        {
            // When you want to show the modal page, just call this method
            // add the event handler for to listen for the modal popping event:
            Crawl.App.Current.ModalPopping += HandleModalPopping;
            _myModalBattleMonsterListPage = new BattleMonsterListPage();
            await Navigation.PushModalAsync(_myModalBattleMonsterListPage);
        }

        // Show CharacterSelect
        private async void ShowModalPageCharacterSelect()
        {
            // When you want to show the modal page, just call this method
            // add the event handler for to listen for the modal popping event:
            Crawl.App.Current.ModalPopping += HandleModalPopping;
            _myModalCharacterSelectPage = new OpeningPage();
            await Navigation.PushModalAsync(_myModalCharacterSelectPage);
        }

        // Show Game Over.
        private async void ShowModalPageGameOver()
        {
            // When you want to show the modal page, just call this method
            // add the event handler for to listen for the modal popping event:
            Crawl.App.Current.ModalPopping += HandleModalPopping;
            _myModalBattleGameOverPage = new BattleOverPage();
            await Navigation.PushModalAsync(_myModalBattleGameOverPage);
        }

        // Add Characters and Monsters to screen.
        public void AllCharactersToScreen()
        {
            StackLayout myStackLayoutCharacter = this.FindByName<StackLayout>("CharacterBox");
            foreach (var data in _viewModel.BattleEngine.CharacterList)
            {
                var temp = new PlayerInfo(data);
                AddPlayerToScreen(temp, myStackLayoutCharacter);
            }

            StackLayout myStackLayoutMonster = this.FindByName<StackLayout>("MonsterBox");
            foreach (var data in _viewModel.BattleEngine.MonsterList)
            {
                var temp = new PlayerInfo(data);
                AddPlayerToScreen(temp, myStackLayoutMonster);
            }
        }

        /// <summary>
        /// Add a Player to the passed in stack layout
        /// </summary>
        /// <param name="data"></param>
        /// <param name="PlayerStackLayout"></param>
        public void AddPlayerToScreen(PlayerInfo data, StackLayout PlayerStackLayout)
        {
            var myName = new Label()
            {
                Text = data.Name,
                Style = (Style)Application.Current.Resources["TeamPlayerText"]
            };

            StackLayout OuterFrame = new StackLayout
            {
                Style = (Style)Application.Current.Resources["TeamPlayerBox"]
            };

            OuterFrame.Children.Add(myName);

            PlayerStackLayout.Children.Add(OuterFrame);
        }
    }
}
