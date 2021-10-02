import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { DetailsComponent } from './views/details/details.component';
import { GameRoomComponent } from './views/game-room/game-room.component';
import { HomeComponent } from './views/home/home.component';
import { TrashComponent } from './views/trash/trash.component';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: HomeComponent,
  },
  {
    path: 'trash',
    pathMatch: 'full',
    component: TrashComponent,
  },
  {
    path: 'details/:id',
    pathMatch: 'full',
    component: DetailsComponent,
  },
  {
    path: 'detailsByName/:name',
    pathMatch: 'full',
    component: DetailsComponent,
  },
  {
    path: 'game-room',
    pathMatch: 'full',
    component: GameRoomComponent,
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
