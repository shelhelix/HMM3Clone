using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Hmm3Clone.Behaviour {
    public class BuildingButton : GameComponent {
        [NotNull] public List<BuildingInfo> Buildings;

        [NotNull] public TMP_Text BuildingName;
        [NotNull] public Button Button;
        [NotNull] public Image FrameBackground;
        [NotNull] public Image BuildingPreview;
        
        ResourceController _resourceController;

        CityState _state;
        
        public void Start() {
            _state = ActiveData.Instance.GetData<CityState>();
            _resourceController = GameController.Instance.GetController<ResourceController>();
            
            InitView();
            Button.onClick.AddListener(ErectBuilding);
        }

        public void OnDestroy() {
            Button.onClick.RemoveAllListeners();
        }

        void InitView() {
            var activeBuildingInfo = GetActiveBuildingInfo();
            BuildingName.text = activeBuildingInfo.Name.ToString();
            FrameBackground.color = _state.IsErected(activeBuildingInfo.Name)
                ? Color.yellow
                : CanErect(activeBuildingInfo, _state)
                    ? Color.green
                    : Color.red;
            BuildingPreview.sprite = activeBuildingInfo.BuildingSprite;
        }
        
        bool CanErect(BuildingInfo buildingInfo, CityState state) {
            return 
                buildingInfo.BuildingCost.TrueForAll(_resourceController.IsEnoughResource)
                && buildingInfo.Dependencies.TrueForAll(x => state.IsErected(x.Name));
        }

        void ErectBuilding() {
            var activeBuildingInfo = GetActiveBuildingInfo();
            _state.ErectedBuildings.Add(activeBuildingInfo.Name);
            InitView();
        }
        
        BuildingInfo GetActiveBuildingInfo() {
            foreach (var building in Buildings) {
                if (!_state.IsErected(building.Name)) {
                    return building;
                }
            }
            return Buildings[Buildings.Count-1];
        }
    }
}
