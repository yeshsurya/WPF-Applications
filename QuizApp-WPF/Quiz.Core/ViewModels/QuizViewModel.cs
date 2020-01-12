﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
namespace Quiz.Core
{
    /// <summary>
    /// The View Model for a main quiz page
    /// </summary>
    public class QuizViewModel : BaseViewModel
    {
        #region Private Members

        // Answers
        private string mAnswerA = "";
        private string mAnswerB = "";
        private string mAnswerC = "";
        private string mAnswerD = "";

        #endregion

        #region Public Properties

        /// <summary>
        /// The name of the user
        /// </summary>
        public string Username => IoC.Application.Username;

        /// <summary>
        /// The user's score in quiz
        /// </summary>
        public int UserScore { get; set; } = 0;

        /// <summary>
        /// The actual question in quiz
        /// </summary>
        public string Question { get; set; } = "";

        /// <summary>
        /// Counter for questions
        /// </summary>
        public int QuestionCount { get; set; } = 0;

        // Answers
        public string AnswerA
        {
            get
            {
                return "Option A: " + mAnswerA;
            }
            set
            {
                mAnswerA = value;
            }
        }
        public string AnswerB
        {
            get
            {
                return "Option B: " + mAnswerB;
            }
            set
            {
                mAnswerB = value;
            }
        }
        public string AnswerC
        {
            get
            {
                return "Option C: " + mAnswerC;
            }
            set
            {
                mAnswerC = value;
            }
        }
        public string AnswerD
        {
            get
            {
                return "Option D: " + mAnswerD;
            }
            set
            {
                mAnswerD = value;
            }
        }

        // Buttons are disabled before quiz starts, true if should be enabled
        public bool IsAnswerAVisible { get; set; } = false;
        public bool IsAnswerBVisible { get; set; } = false;
        public bool IsAnswerCVisible { get; set; } = false;
        public bool IsAnswerDVisible { get; set; } = false;

        /// <summary>
        /// The list of question objects
        /// </summary>
        public List<QuizQuestionModel> QuestionList = new List<QuizQuestionModel>();

        /// <summary>
        /// True if start button should be shown
        /// </summary>
        public bool IsStartVisible { get; set; } = true;

        #endregion

        #region Commands

        /// <summary>
        /// The command to call if user choose answer A
        /// </summary>
        public ICommand AnswerACommand { get; set; }

        /// <summary>
        /// The command to call if user choose answer B
        /// </summary>
        public ICommand AnswerBCommand { get; set; }

        /// <summary>
        /// The command to call if user choose answer C
        /// </summary>
        public ICommand AnswerCCommand { get; set; }

        /// <summary>
        /// The command to call if user choose answer D
        /// </summary>
        public ICommand AnswerDCommand { get; set; }

        /// <summary>
        /// The command to start the quiz
        /// </summary>
        public ICommand StartCommand { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public QuizViewModel()
        {
            // Create commands
            AnswerACommand = new RelayCommand(async () => await UserAnswerAsync('A'));
            AnswerBCommand = new RelayCommand(async () => await UserAnswerAsync('B'));
            AnswerCCommand = new RelayCommand(async () => await UserAnswerAsync('C'));
            AnswerDCommand = new RelayCommand(async () => await UserAnswerAsync('D'));
            StartCommand = new RelayCommand(StartQuiz);
        }

        #endregion

        #region Command Methods

        /// <summary>
        /// Handles user's answer and checks if it is correct
        /// </summary>
        /// <param name="answer">User answer as a letter (A, B, C or D)</param>
        /// <returns></returns>
        private async Task UserAnswerAsync(char answer)
        {
            await Task.Delay(100);

            // Check if user answered correctly
            if (QuestionList[QuestionCount - 1].RightAnswer == answer)
            {
                // If yes, add points to his score
                UserScore += QuestionList[QuestionCount - 1].Points;
            }

            // Add user answer to list
            AnswersViewModel.Instance.Items.Add
            (
                new AnswersItemViewModel
                {
                    AnswerLetter = answer,
                    AnswerNumber = QuestionCount
                }
            );

            // Check if it was last question, if yes end quiz
            if (QuestionCount == QuestionList.Count) EndQuiz();
            else
            {
                // Step into next question
                QuestionCount++;
                ShowQuestion(QuestionCount);
            }
        }

        /// <summary>
        /// Run to start the Quiz
        /// </summary>
        /// <returns></returns>
        private void StartQuiz()
        {
            // Start the timer.
            //HelpPage.Instance.startTimer();

            // Hide start button
            IsStartVisible = false;

            // At the start, the first question is question number 1
            QuestionCount = 1;

            // Download questions from file
            DownloadQuestions();

            // Show the first question
            ShowQuestion(QuestionCount);

            // Enable buttons to allow user to answer a question
            IsAnswerAVisible = true;
            IsAnswerBVisible = true;
            IsAnswerCVisible = true;
            IsAnswerDVisible = true;
        }

        #endregion

        #region Private Helpers

        /// <summary>
        /// Run to save results, show them to user and end the Quiz
        /// </summary>
        /// <param name="number">The number of questions that user have answered</param>
        private void EndQuiz()
        {
            // Save quiz results to file 
            SaveQuizToFile();

            // Show message box with user's score
            IoC.UI.ShowMessage(new MessageBoxDialogViewModel
            {
                Title = "End of the quiz",
                Message = $"You got{ this.UserScore } points!",
                OkText = "Exit"
            });

            // Close the application
            IoC.UI.CloseApplication();
        }

        /// <summary>
        /// Called to load questions to the Quiz
        /// </summary>
        private void DownloadQuestions()
        {
            // Load file array
            var fileText = new FilePath().QuestionsFile;

            // Prepare variables
            int i = 1, points;
            string question = "", ansA = "", ansB = "", ansC = "", ansD = "";
            char rightAns;

            // Loop though lines
            foreach (string line in fileText)
            {
                switch (i)
                {
                    // Catch every line to separate variables
                    case 1:
                        question = line;
                        break;
                    case 2:
                        ansA = line;
                        break;
                    case 3:
                        ansB = line;
                        break;
                    case 4:
                        ansC = line;
                        break;
                    case 5:
                        ansD = line;
                        break;
                    case 6:
                        {
                            rightAns = line[0];
                            points = Int32.Parse(line[1].ToString());

                            // We have everything we need, lets create new question
                            QuizQuestionModel NewQuestion = new QuizQuestionModel(question, ansA, ansB, ansC, ansD, rightAns, points);

                            // And add it to the list
                            QuestionList.Add(NewQuestion);

                            // Reset the iterator for next question
                            i = 0;
                        }
                        break;
                }
                i++;
            }
        }

        /// <summary>
        /// Shows question on the screen
        /// </summary>
        /// <param name="number">Indicate which question should be shown</param>
        private void ShowQuestion(int number)
        {
            // If method was miscalled with not valid number, return
            if (number <= 0) return;

            // Set question parameters to view model properties
            this.Question = QuestionList[number - 1].Question;
            this.AnswerA = QuestionList[number - 1].AnswerA;
            this.AnswerB = QuestionList[number - 1].AnswerB;
            this.AnswerC = QuestionList[number - 1].AnswerC;
            this.AnswerD = QuestionList[number - 1].AnswerD;
        }

        /// <summary>
        /// Saves user score into file
        /// </summary>
        private void SaveQuizToFile()
        {
            // Get the list of question numbers in string
            string questionNumbers = MakeQuestionNumbers();
            // Get the list of right answers in string
            string rightAnswers = MakeRightAnswers();
            // Get the list of user's answers in string
            string userAnswers = MakeUserAnswers();
            // Get the list of possible/collectable points in string
            string collectablePoints = MakeCollectablePoints();

            // Create an array with every list
            string[] stringArray = new string[]
            {
                "Participant:\t\t " + this.Username,
                questionNumbers,
                rightAnswers,
                userAnswers,
                collectablePoints,
                "Number of points scored:" + this.UserScore
            };

            // Output it into file
            File.WriteAllLines(this.Username + ".txt", stringArray);
        }

        #region File Save Helpers

        private string MakeQuestionNumbers()
        {
            string localQuestionNumbers = "Question number:\t\t";
            if (QuestionCount > 10)
            {
                for (int i = 1; i <= 10; i++) localQuestionNumbers += " " + i + " ";
                for (int i = 11; i <= QuestionCount; i++) localQuestionNumbers += i + " ";
            }
            else
            {
                for (int i = 1; i <= QuestionCount; i++) localQuestionNumbers += " " + i + " ";
            }
            return localQuestionNumbers;
        }

        private string MakeRightAnswers()
        {
            string localRightAnswers = "Correct ODP:\t\t";
            for (int i = 0; i < QuestionCount; i++) localRightAnswers += " " + QuestionList[i].RightAnswer + " ";
            return localRightAnswers;
        }

        private string MakeUserAnswers()
        {
            string localUserAnswers = "Obtained ODP:\t\t";
            for (int i = 0; i < QuestionCount; i++) localUserAnswers += " " + AnswersViewModel.Instance.Items[i].AnswerLetter + " ";
            return localUserAnswers;
        }

        private string MakeCollectablePoints()
        {
            string localCollectablePoints = "Points to get:\t";
            for (int i = 0; i < QuestionCount; i++) localCollectablePoints += " " + QuestionList[i].Points + " ";
            return localCollectablePoints;
        }

        #endregion

        #endregion
    }
}
