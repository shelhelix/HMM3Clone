using System;
using System.Collections.Generic;
using GameComponentAttributes;
using GameComponentAttributes.Attributes;
using Hmm3Clone.Controller;
using Hmm3Clone.State;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Hmm3Clone.Behaviour {
    public class BuildingButton : GameComponent {
        [NotNull] public List<BuildingInfo> Buildings;

        [NotNull] public TMP_Text BuildingName;
        [NotNull] public Button Button;
        [NotNull] public Image FrameBackground;
        [NotNull] public Image BuildingPreview;

        [Inject] CityController _cityController;
        [Inject] TurnController _turnController;

        [Inject] CityState _state;

        BuildingInfo _activeBuildingInfo;

        bool _isInit;
        
        void Start() {
            if (_isInit) {
                return;
            }
                
            Button.onClick.AddListener(ErectBuilding);
            _cityController.OnBuildingsChanged += Refresh;
            _turnController.OnTurnChanged      += OnTurnChanged;

            _isInit = true;
        }

        void OnDestroy() {
            Button.onClick.RemoveAllListeners();
            _cityController.OnBuildingsChanged -= Refresh;
            _turnController.OnTurnChanged      -= OnTurnChanged;
        }

        void OnTurnChanged(int turn) {
            Init();
        }

        void Refresh() {
            Init();
        }

        void OnEnable() {
            if (!_isInit) {
                Start();
            }
            Init();
        }

        void Init() {
            _activeBuildingInfo = GetActiveBuildingInfo();
            InitView();
        }

        void InitView() {
            BuildingName.text = _activeBuildingInfo.Name.ToString();
            FrameBackground.color = _cityController.IsErected(_state.CityName, _activeBuildingInfo.Name)
                ? Color.yellow
                : _cityController.CanErectBuilding(_state.CityName, _activeBuildingInfo.Name)
                    ? Color.green
                    : Color.red;
            BuildingPreview.sprite = _activeBuildingInfo.BuildingSprite;
        }

        void ErectBuilding() {
            _cityController.ErectBuilding(_state.CityName, _activeBuildingInfo.Name);
        }
        
        BuildingInfo GetActiveBuildingInfo() {
            foreach (var building in Buildings) {
                if (!_cityController.IsErected(_state.CityName, building.Name)) {
                    return building;
                }
            }
            return Buildings[Buildings.Count-1];
        }
    }
}
