// ng g m [module_name] --routing [komanda za generisanje modula]

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { WelcomeComponent } from './home/welcome/welcome.component'; // Importanje komponente da je mozemo koristiti.
import { ShellComponent } from './home/shell/shell.component';
import { PageNotFoundComponent } from './home/page-not-found.component';
import { AuthGuard } from '@auth/guards/auth.guard';
import { Role } from '@core/models/auth/role';

const routes: Routes = [
  {
    path: '', component: ShellComponent, // Ovo prikazuje glavni okvir stranice, sve dok je stranica prazna prikazi date pod rute.
    children: [
      { path: 'welcome', component: WelcomeComponent }, // Ako je welcome, korisnika ce prebaciti na datu komponentu.
      { path: '', redirectTo: 'welcome', pathMatch: 'full' }, // Ovo pravi preusmjerenje s prazne stranice na welcome.
      {
        path: 'movies',
        canActivate: [AuthGuard],
        loadChildren: () => import('./movie/movie.module').then(m => m.MovieModule)
      },
      {
        path: '',
        canActivate: [AuthGuard],
        loadChildren: () => import('./user-profile/profile.module').then(m => m.ProfileModule)
      },
      {
        path: 'play',
        canActivate: [AuthGuard],
        loadChildren: () => import('./player/player.module').then(m => m.PlayerModule)
      },
      { path: 'auth', loadChildren: () => import('./auth/auth.module').then(m => m.AuthModule)},
    ]
  },
  { path: '**', component: PageNotFoundComponent } // Kada korisnik unese ne postojecu putanju prebacit ce ga na ovu komponentu.
];

@NgModule({
  imports: [RouterModule.forRoot(routes)], // Postavlja glavne rute za nasu aplikaciju.
  exports: [RouterModule]
})
export class AppRoutingModule { }
