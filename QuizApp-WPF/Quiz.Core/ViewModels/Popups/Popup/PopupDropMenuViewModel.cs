using System.Collections.Generic;

namespace Quiz.Core
{
    /// <summary>
    /// A view model for any popup menus
    /// </summary>
    public class PopupDropMenuViewModel : BasePopupViewModel
    {
        #region Constructor

        /// <summary>
        /// Default constructor
        /// </summary>
        public PopupDropMenuViewModel()
        {
            // Add items to popup menu
            Content = new MenuViewModel
            {
                Items = new List<MenuItemViewModel>(new[]
                {
                    new MenuItemViewModel { Text = "Help", Icon = IconType.Help },
                    new MenuItemViewModel { Text = "Answer history", Icon = IconType.History },
                })
            };
        }

        #endregion


    }
}
