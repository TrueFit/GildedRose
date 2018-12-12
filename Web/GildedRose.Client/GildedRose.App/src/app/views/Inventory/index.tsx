import * as React from "react";
import { Header } from "app/components/Header";
import { Footer } from "app/components/Footer";
import { InventoryGrid } from "../../components/InventoryGrid/InventoryGrid";
import { GridData, InventoryModel } from "models";

export class InventoryView extends React.Component<{}> {
  // tslint:disable-next-line:member-ordering
  constructor(props: {}) {
    super(props);
  }

  public render(): JSX.Element {
    const data = [
      {
        identifier: "88bdb452-e23d-4b70-b07e-c9f3f3f0d1a9",
        name: "+5 Dexterity Vest",
        categoryId: 8,
        categoryName: "Armor",
        initialQuality: 20,
        currentQuality: 18,
        maxQuality: 50,
        sellIn: 8,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "49ca5c68-4fbd-4ef6-9fe8-e086c6ef81e6",
        name: "Elixir of the Mongoose",
        categoryId: 6,
        categoryName: "Potion",
        initialQuality: 7,
        currentQuality: 5,
        maxQuality: 50,
        sellIn: 3,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "95a79f27-61e1-4c70-8039-0fdd703fc6e9",
        name: "Giant Slayer",
        categoryId: 5,
        categoryName: "Conjured",
        initialQuality: 50,
        currentQuality: 46,
        maxQuality: 50,
        sellIn: 13,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "58c1acc3-0ce7-4a06-86f3-5052d223e48d",
        name: "TAFKAL80ETC Concert",
        categoryId: 4,
        categoryName: "Backstage Passes",
        initialQuality: 20,
        currentQuality: 0,
        maxQuality: 50,
        sellIn: 13,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "dddd657e-9617-4c82-bdc5-0ab53b5a6398",
        name: "Aged Brie",
        categoryId: 2,
        categoryName: "Food",
        initialQuality: 10,
        currentQuality: 12,
        maxQuality: 50,
        sellIn: 48,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "eea3072c-1579-469a-81c9-f2ba6302a0ca",
        name: "Hand of Ragnaros",
        categoryId: 3,
        categoryName: "Sulfuras",
        initialQuality: 80,
        currentQuality: 80,
        maxQuality: 80,
        sellIn: 78,
        isLegendary: true,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "15298b70-b493-43a8-b01e-2cccf5514a89",
        name: "Full Plate Mail",
        categoryId: 8,
        categoryName: "Armor",
        initialQuality: 50,
        currentQuality: 48,
        maxQuality: 50,
        sellIn: 48,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "f3205dfd-55fe-4cd5-8070-b259e9db2f7b",
        name: "Sword",
        categoryId: 1,
        categoryName: "Weapon",
        initialQuality: 50,
        currentQuality: 48,
        maxQuality: 50,
        sellIn: 28,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "4bb48e57-245a-419e-880c-1b701dbb35c2",
        name: "Wooden Shield",
        categoryId: 8,
        categoryName: "Armor",
        initialQuality: 30,
        currentQuality: 28,
        maxQuality: 50,
        sellIn: 8,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "3884fae6-6dd9-4e4f-bc04-bcb90f18dae7",
        name: "Belt of Giant Strength",
        categoryId: 5,
        categoryName: "Conjured",
        initialQuality: 40,
        currentQuality: 36,
        maxQuality: 50,
        sellIn: 18,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "8686e4d9-a43c-4c46-bb72-81e9454609c6",
        name: "Storm Hammer",
        categoryId: 5,
        categoryName: "Conjured",
        initialQuality: 50,
        currentQuality: 46,
        maxQuality: 50,
        sellIn: 18,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "35096084-af46-4e40-932e-655aab9bac00",
        name: "Axe",
        categoryId: 1,
        categoryName: "Weapon",
        initialQuality: 50,
        currentQuality: 48,
        maxQuality: 50,
        sellIn: 38,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "e2055eae-6d1f-45a9-9701-7c5fbf563e20",
        name: "I am Murloc",
        categoryId: 4,
        categoryName: "Backstage Passes",
        initialQuality: 10,
        currentQuality: 0,
        maxQuality: 50,
        sellIn: 18,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "e12e2698-79b6-4f15-bfa2-a1fab047aa27",
        name: "Halberd",
        categoryId: 1,
        categoryName: "Weapon",
        initialQuality: 40,
        currentQuality: 38,
        maxQuality: 50,
        sellIn: 58,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "f1ab5859-cd8a-46b2-9520-dd631cbde700",
        name: "Potion of Healing",
        categoryId: 6,
        categoryName: "Potion",
        initialQuality: 10,
        currentQuality: 8,
        maxQuality: 50,
        sellIn: 8,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "02223851-8f9a-4b55-a925-e59be7855413",
        name: "Aged Milk",
        categoryId: 2,
        categoryName: "Food",
        initialQuality: 20,
        currentQuality: 18,
        maxQuality: 50,
        sellIn: 18,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "a5091448-6848-4247-8d85-2ee5a6a8defb",
        name: "Bag of Holding",
        categoryId: 7,
        categoryName: "Misc",
        initialQuality: 50,
        currentQuality: 48,
        maxQuality: 50,
        sellIn: 8,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "e52e05f0-aba1-44e8-8003-435bb20bd660",
        name: "Mutton",
        categoryId: 2,
        categoryName: "Food",
        initialQuality: 10,
        currentQuality: 8,
        maxQuality: 50,
        sellIn: 8,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "960fa030-70ac-4ff7-8ad8-07cea6ca84ce",
        name: "Raging Ogre",
        categoryId: 4,
        categoryName: "Backstage Passes",
        initialQuality: 10,
        currentQuality: 18,
        maxQuality: 50,
        sellIn: 8,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
      {
        identifier: "fc22fba7-7f78-4ac5-a155-d89a40a1940a",
        name: "Cheese",
        categoryId: 2,
        categoryName: "Food",
        initialQuality: 5,
        currentQuality: 3,
        maxQuality: 50,
        sellIn: 3,
        isLegendary: false,
        created: "2018-11-28T01:19:22",
        createdBy: 1,
        modified: undefined,
        modifiedBy: undefined,
      },
    ];

    // const onPageSizeChange = (newPageSize: number) => {
    //   alert("page size changed");
    // };

    // const onPageChange = (page: number) => {
    //   alert("page size changed");
    // };

    const loginStyle = {
      textAlign: "center",
    } as React.CSSProperties;

    const dto = data.map((x: InventoryModel) => {
      return {
        id: x.identifier,
        name: x.name,
        categoryId: x.categoryId,
        categoryName: x.categoryName,
        quality: {
          current: x.currentQuality,
          initial: x.initialQuality,
          max: x.maxQuality,
        },
        sellIn: x.sellIn,
        isLegendary: x.isLegendary,
      } as GridData;
    });

    return (
      <>
        <Header title={"Login Screen"} isAuthenticated={false} />
        <div>
          <div style={loginStyle}>
            <div>
              <InventoryGrid
                Data={dto}
                PageSize={10}
                TotalPages={10}
                PageNumber={1}
              />
            </div>

          </div>
        </div>
        <Footer language={"© Copyright 2018 GildedRose LLC"} />
      </>
    );
  }
}
