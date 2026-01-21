//using System;
//using Hawky;
//using Hawky.Scene;
//using ColorFight;


//public partial class RDM : RuntimeSingleton<RDM>
//    {
//        public SettingPopupModel SettingPopupModel;
//        public ReplayLevelPopupModel ReplayLevelPopupModel;
//        public TutorialPopupModel TutorialPopupModel;
//        public LoadingPopupModel LoadingPopupModel;
//        public WinPopupModel WinPopupModel;
//        public LosePopupModel LosePopupModel;
//    }

//namespace ColorFight {
//    public class SettingPopupModel
//    {
//        public class SettingPopupReturnData : ReturnData
//        {

//        }
//        public SettingPopupReturnData ReturnData;

//        public SettingPopupModel()
//        {
//            ReturnData = new SettingPopupReturnData();
//        }
//    }

//    public class ReplayLevelPopupModel
//    {
//        public class ReplayLevelPopupReturnData : ReturnData
//        {

//        }
//        public ReplayLevelPopupReturnData ReturnData;

//        public ReplayLevelPopupModel()
//        {
//            ReturnData = new ReplayLevelPopupReturnData();
//        }
//    }

//    public class TutorialPopupModel
//    {
//        public class TutorialPopupReturnData : ReturnData
//        {

//        }
//        public TutorialPopupReturnData ReturnData;

//        public TutorialPopupModel()
//        {
//            ReturnData = new TutorialPopupReturnData();
//        }
//    }

//    public class LoadingPopupModel
//    {
//        public class LoadingPopupReturnData : ReturnData
//        {
//            public Action OnComptete;
//        }
//        public LoadingPopupReturnData ReturnData;

//        public LoadingPopupModel()
//        {
//            ReturnData = new LoadingPopupReturnData();
//        }
//    }

//    public class WinPopupModel
//    {
//        public class WinPopupReturnData : ReturnData
//        {
//            public Action OnComptete;
//        }
//        public WinPopupReturnData ReturnData;

//        public WinPopupModel()
//        {
//            ReturnData = new WinPopupReturnData();
//        }
//    }

//    public class LosePopupModel
//    {
//        public class LosePopupReturnData : ReturnData
//        {
//            public Action OnComptete;
//        }
//        public LosePopupReturnData ReturnData;

//        public LosePopupModel()
//        {
//            ReturnData = new LosePopupReturnData();
//        }
//    }
//}
