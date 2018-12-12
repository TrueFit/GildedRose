import * as proxy from "core/services/apiProxy";
import { InventoryModel } from "models";
import * as moment from "moment";

export async function getInventoryByDateViewed(viewByDate: Date): Promise<InventoryModel[]> {
  const momentDate = moment(viewByDate).format("MM/DD/YYYY");
  return await proxy.get<InventoryModel[]>(`/Item/getall/?viewByDate=${momentDate}`);
}
