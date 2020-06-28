using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 一般客人AI
/// </summary>
public class GuestAIController : AIController
{
    protected override void ConstructFSM()
    {
        //点餐排队
        QueueUpDinnerState queueDinner = new QueueUpDinnerState();
        queueDinner.AddTransition(Transition.QueueUpDinnerOver, FSMStateID.TakeOrder);
        queueDinner.AddTransition(Transition.QueueUpDinnerOver, FSMStateID.Leave);
        //点餐
        TakeOrderState takeOrder = new TakeOrderState();
        takeOrder.AddTransition(Transition.TakeOrderOver, FSMStateID.HaveDinner);
        takeOrder.AddTransition(Transition.TakeOrderOver, FSMStateID.Leave);
        //大胃王
        TakeOrderRepeatState repeatOrder = new TakeOrderRepeatState();
        repeatOrder.AddTransition(Transition.TakeOrderRepeatOver, FSMStateID.HaveDinner);
        //用餐
        HaveDinnerState haveDinner = new HaveDinnerState();
        haveDinner.AddTransition(Transition.HaveDinnerOver, FSMStateID.QueueUpCoffee);
        haveDinner.AddTransition(Transition.HaveDinnerOver, FSMStateID.QueueUpWine);
        haveDinner.AddTransition(Transition.HaveDinnerOver, FSMStateID.QueueUpDessert);
        haveDinner.AddTransition(Transition.HaveDinnerOver, FSMStateID.Leave);
        haveDinner.AddTransition(Transition.HaveDinnerOver, FSMStateID.TakeDinnerRepeat);
        haveDinner.AddTransition(Transition.HaveDinnerOver, FSMStateID.GuestRandomMove);
        //消费排队
        QueueUpWineState wineQueue = new QueueUpWineState();
        wineQueue.AddTransition(Transition.QueueUpWineOver, FSMStateID.WineConsumption);
        wineQueue.AddTransition(Transition.QueueUpWineOver, FSMStateID.Leave);
        QueueUpCoffeeState coffeeQueue = new QueueUpCoffeeState();
        coffeeQueue.AddTransition(Transition.QueueUpCoffeeOver, FSMStateID.CoffeeConsumption);
        coffeeQueue.AddTransition(Transition.QueueUpCoffeeOver, FSMStateID.Leave);
        QueueUpDessertState dessertQueue = new QueueUpDessertState();
        dessertQueue.AddTransition(Transition.QueueUpDessertOver, FSMStateID.DessertConsumption);
        dessertQueue.AddTransition(Transition.QueueUpDessertOver, FSMStateID.Leave);
        //酒水消费
        WineConsumptionState wineConsump = new WineConsumptionState();
        wineConsump.AddTransition(Transition.WineConsumptionOver, FSMStateID.QueueUpCoffee);
        wineConsump.AddTransition(Transition.WineConsumptionOver, FSMStateID.QueueUpDessert);
        wineConsump.AddTransition(Transition.WineConsumptionOver, FSMStateID.GuestRandomMove);
        wineConsump.AddTransition(Transition.WineConsumptionOver, FSMStateID.GardenRandomMove);
        wineConsump.AddTransition(Transition.WineConsumptionOver, FSMStateID.Leave);
        //咖啡消费
        CoffeeConsumptionState coffeeComsump = new CoffeeConsumptionState();
        coffeeComsump.AddTransition(Transition.CoffeeConsumptionOver, FSMStateID.QueueUpDessert);
        coffeeComsump.AddTransition(Transition.CoffeeConsumptionOver, FSMStateID.QueueUpWine);
        coffeeComsump.AddTransition(Transition.CoffeeConsumptionOver, FSMStateID.GuestRandomMove);
        coffeeComsump.AddTransition(Transition.CoffeeConsumptionOver, FSMStateID.GardenRandomMove);
        coffeeComsump.AddTransition(Transition.CoffeeConsumptionOver, FSMStateID.Leave);
        //糖果消费
        DessertConsumptionState dessertComsump = new DessertConsumptionState();
        dessertComsump.AddTransition(Transition.DessertConsumptionOver, FSMStateID.QueueUpCoffee);
        dessertComsump.AddTransition(Transition.DessertConsumptionOver, FSMStateID.QueueUpWine);
        dessertComsump.AddTransition(Transition.DessertConsumptionOver, FSMStateID.GuestRandomMove);
        dessertComsump.AddTransition(Transition.DessertConsumptionOver, FSMStateID.GardenRandomMove);
        dessertComsump.AddTransition(Transition.DessertConsumptionOver, FSMStateID.Leave);
        //餐厅随机移动
        GuestRandomWalkState randomWalk = new GuestRandomWalkState();
        randomWalk.AddTransition(Transition.RandomMoveOver, FSMStateID.QueueUpWine);
        randomWalk.AddTransition(Transition.RandomMoveOver, FSMStateID.QueueUpCoffee);
        randomWalk.AddTransition(Transition.RandomMoveOver, FSMStateID.QueueUpDessert);
        randomWalk.AddTransition(Transition.RandomMoveOver, FSMStateID.Leave);
        //花园
        GardenRandomMoveState gardenMove = new GardenRandomMoveState();
        gardenMove.AddTransition(Transition.GardenRandomMoveOver, FSMStateID.GardenFlowers);
        gardenMove.AddTransition(Transition.GardenRandomMoveOver, FSMStateID.Leave);
        GardenFlowerState admireFlower = new GardenFlowerState();
        admireFlower.AddTransition(Transition.GardenFlowersOver, FSMStateID.GardenRandomMove);
        admireFlower.AddTransition(Transition.GardenFlowersOver, FSMStateID.NotionStoreRandomMove);
        admireFlower.AddTransition(Transition.GardenFlowersOver, FSMStateID.Leave);
        //便利店
        NotionStoreRandomMoveState notionMove = new NotionStoreRandomMoveState();
        notionMove.AddTransition(Transition.NotionRandomMoveOver, FSMStateID.NotionStoreBuy);
        notionMove.AddTransition(Transition.NotionRandomMoveOver, FSMStateID.Leave);
        NotionStoreBuyState notionBuy = new NotionStoreBuyState();
        notionBuy.AddTransition(Transition.NotionBuyOver, FSMStateID.NotionStoreRandomMove);
        notionBuy.AddTransition(Transition.NotionBuyOver, FSMStateID.Leave);
        //离开
        LeaveState leave = new LeaveState();

        AddFSMState(queueDinner);
        AddFSMState(takeOrder);
        AddFSMState(repeatOrder);
        AddFSMState(haveDinner);
        AddFSMState(wineQueue);
        AddFSMState(coffeeQueue);
        AddFSMState(dessertQueue);
        AddFSMState(wineConsump);
        AddFSMState(coffeeComsump);
        AddFSMState(dessertComsump);
        AddFSMState(randomWalk);
        AddFSMState(gardenMove);
        AddFSMState(admireFlower);
        AddFSMState(notionMove);
        AddFSMState(notionBuy);
        AddFSMState(leave);
    }
}
