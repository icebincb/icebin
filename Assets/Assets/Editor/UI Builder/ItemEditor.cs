// using System;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.UIElements;
// using UnityEditor.UIElements;
// using System.Collections.Generic;
// using System.Linq;
// public class ItemEditor : EditorWindow
// {
//     private ItemList_SO _itemListSo;
//
//     private List<ItemDetails> itemlist = new List<ItemDetails>();
//
//     private VisualTreeAsset itemRowTemplate;
//
//     private ListView itemListView;
//
//     private ScrollView _scrollView;
//
//     private ItemDetails activeItem;
//     //默认预览图片
//     private Sprite defaultIcon;
//
//     private VisualElement iconPreview;
//     
//     [MenuItem("Tools/ItemEditor")]
//     public static void ShowExample()
//     {
//         ItemEditor wnd = GetWindow<ItemEditor>();
//         wnd.titleContent = new GUIContent("ItemEditor");
//     }
//
//     public void CreateGUI()
//     {
//         // Each editor window contains a root VisualElement object
//        VisualElement root = rootVisualElement;
//
//         // VisualElements objects can contain other VisualElement following a tree hierarchy.
//        // VisualElement label = new Label("Hello World! From C#");
//         //root.Add(label);
//
//         // Import UXML
//         var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemEditor.uxml");
// //        VisualElement labelFromUXML = visualTree.Instantiate();
//   //      root.Add(labelFromUXML);
//         itemRowTemplate =
//             AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/UI Builder/ItemRowTemplate.uxml");
//         itemListView = root.Q<VisualElement>("ItemList").Q<ListView>("ListView");
//         _scrollView = root.Q<ScrollView>("ItemDetails");
//         iconPreview = _scrollView.Q<VisualElement>("Icon");
//         //拿默认Icon图片
//         defaultIcon = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/M Studio/Art/Items/Icons/icon_M.png");
//
//         //获得按键
//         root.Q<Button>("AddButton").clicked += OnAddItemClicked;
//         root.Q<Button>("DeleteButton").clicked += OnDeleteClicked;
//        LoadData();
//        GenerateListView();
//     }
//
//     private void OnAddItemClicked()
//     {
//         ItemDetails newItem = new ItemDetails();
//         newItem.itemname = "NewItem";
//         newItem.itemID = 1000 + itemlist.Count;
//         itemlist.Add(newItem);
//         itemListView.Rebuild();
//         
//     }
//     private void OnDeleteClicked()
//     {
//         itemlist.Remove(activeItem);
//         itemListView.Rebuild();
//         _scrollView.visible = false;
//     }
//
//     private void LoadData()
//     {
//        var data= AssetDatabase.FindAssets("ItemList_SO");
//        if (data.Length > 1)
//        {
//            var path = AssetDatabase.GUIDToAssetPath(data[0]);
//            _itemListSo = AssetDatabase.LoadAssetAtPath(path, typeof(ItemList_SO)) as ItemList_SO;
//        }
//
//        itemlist = _itemListSo.item;
//        //如果不标记则无法保存数据
//        EditorUtility.SetDirty(_itemListSo);
// //       Debug.Log(itemlist[0].itemname);
//        
//       
//        
//     }
//
//     private void GenerateListView()
//     {
//
//         Func<VisualElement> makeItem = () => itemRowTemplate.CloneTree();
//         Action<VisualElement, int> bindItem = (e, i) =>
//         {
//             if (i < itemlist.Count&&itemlist[i]!=null)
//             { 
//                 if(itemlist[i].itemIcon!=null)
//                     e.Q<VisualElement>("icon").style.backgroundImage = itemlist[i].itemIcon.texture;
//                 e.Q<Label>("Name").text = itemlist[i] == null ? "NO ITEM" : itemlist[i].itemname;
//             }
//         };
//
//         itemListView.itemsSource = itemlist;
//         itemListView.makeItem = makeItem;
//         itemListView.bindItem = bindItem;
//         itemListView.onSelectionChange += OnListSelectionChange;
//         _scrollView.visible = false;
//     }
//
//     private void OnListSelectionChange(IEnumerable<object> obj)
//     {
//         activeItem = obj.First() as ItemDetails;
//         GetItemDetails();
//         _scrollView.visible = true;
//     }
//
//     private void GetItemDetails()
//     {
//         _scrollView.MarkDirtyRepaint();
//
//         _scrollView.Q<IntegerField>("ID").value = activeItem.itemID;
//         _scrollView.Q<IntegerField>("ID").RegisterValueChangedCallback(evt =>
//         {
//             activeItem.itemID = evt.newValue;
//         });
//
//         _scrollView.Q<TextField>("Name").value = activeItem.itemname;
//         _scrollView.Q<TextField>("Name").RegisterValueChangedCallback(evt =>
//         {
//             activeItem.itemname = evt.newValue;
//             itemListView.Rebuild();
//         });
//
//         iconPreview.style.backgroundImage = activeItem.itemIcon == null ? defaultIcon.texture : activeItem.itemIcon.texture;
//         _scrollView.Q<ObjectField>("Icon").value = activeItem.itemIcon;
//         _scrollView.Q<ObjectField>("Icon").RegisterValueChangedCallback(evt =>
//         {
//             Sprite newIcon = evt.newValue as Sprite;
//             activeItem.itemIcon = newIcon;
//
//             iconPreview.style.backgroundImage = newIcon == null ? defaultIcon.texture : newIcon.texture;
//             itemListView.Rebuild();
//         });
//
//         //其他所有变量的绑定
//         _scrollView.Q<ObjectField>("OnWorldIcon").value = activeItem.itemOnWorldSprite;
//         _scrollView.Q<ObjectField>("OnWorldIcon").RegisterValueChangedCallback(evt =>
//         {
//             activeItem.itemOnWorldSprite = (Sprite)evt.newValue;
//         });
//
//         _scrollView.Q<EnumField>("Type").Init(activeItem.itemType);
//         _scrollView.Q<EnumField>("Type").value = activeItem.itemType;
//         _scrollView.Q<EnumField>("Type").RegisterValueChangedCallback(evt =>
//         {
//             activeItem.itemType = (ItemType)evt.newValue;
//         });
//
//         _scrollView.Q<TextField>("Description").value = activeItem.itemDescription;
//         _scrollView.Q<TextField>("Description").RegisterValueChangedCallback(evt =>
//         {
//             activeItem.itemDescription = evt.newValue;
//         });
//
//         _scrollView.Q<IntegerField>("UseRadius").value = activeItem.itemUseRadius;
//         _scrollView.Q<IntegerField>("UseRadius").RegisterValueChangedCallback(evt =>
//         {
//             activeItem.itemUseRadius = evt.newValue;
//         });
//
//         _scrollView.Q<Toggle>("CanPickUp").value = activeItem.canPickedup;
//         _scrollView.Q<Toggle>("CanPickUp").RegisterValueChangedCallback(evt =>
//         {
//             activeItem.canPickedup = evt.newValue;
//         });
//
//         _scrollView.Q<Toggle>("CanDropped").value = activeItem.canDropped;
//         _scrollView.Q<Toggle>("CanDropped").RegisterValueChangedCallback(evt =>
//         {
//             activeItem.canDropped = evt.newValue;
//         });
//
//         _scrollView.Q<Toggle>("CanCarried").value = activeItem.canCarried;
//         _scrollView.Q<Toggle>("CanCarried").RegisterValueChangedCallback(evt =>
//         {
//             activeItem.canCarried = evt.newValue;
//         });
//
//         _scrollView.Q<IntegerField>("itemPrice").value = activeItem.itemPrice;
//         _scrollView.Q<IntegerField>("itemPrice").RegisterValueChangedCallback(evt =>
//         {
//             activeItem.itemPrice = evt.newValue;
//         });
//
//         _scrollView.Q<Slider>("sellPercentage").value = activeItem.sellPercentage;
//         _scrollView.Q<Slider>("sellPercentage").RegisterValueChangedCallback(evt =>
//         {
//             activeItem.sellPercentage = evt.newValue;
//         });
//     }
// }