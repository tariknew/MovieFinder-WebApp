import { NgModule } from '@angular/core';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MenuComponent } from './home/menu/menu.component';
import { AppComponent } from './app.component';
import { MaterialModule } from './material.module'; // Biblioteka koja nam omogucava da koristimo mat-toolbar [ng add @angular/material]
import { WelcomeComponent } from './home/welcome/welcome.component';
import { BrowserModule } from '@angular/platform-browser';
import { ShellComponent } from './home/shell/shell.component';
import { SharedModule } from '@shared/shared.module';
import { PageNotFoundComponent } from './home/page-not-found.component';
import { TokenInterceptor } from 'app/auth/interceptors/token.interceptor';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { ToastrModule } from 'ngx-toastr';
import { AppRoutingModule } from './app-routing.module';
import { MatBadgeModule } from '@angular/material/badge';

const ToastrOptions = {
  positionClass: 'toast-bottom-right',
  closeButton: true,
  progressBar: true
};

@NgModule({
  declarations: [
    AppComponent,
    MenuComponent,
    WelcomeComponent,
    ShellComponent,
    PageNotFoundComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    ToastrModule.forRoot(ToastrOptions),
    HttpClientModule,
    MaterialModule,
    MatBadgeModule,
    SharedModule,
    AppRoutingModule // should be imported last
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
