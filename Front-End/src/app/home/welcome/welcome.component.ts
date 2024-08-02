import {Component, OnInit} from '@angular/core';
@Component({
  selector: 'app-welcome',
  templateUrl: './welcome.component.html',
  styleUrls: ['./welcome.component.css']
})
// Nakon sto dodamo trenutno komponentu u "app.module.ts". Onda klasu mozemo definisati na ovaj nacin.
export class WelcomeComponent implements OnInit {
  constructor(
  ) {
  }
  ngOnInit(): void {
  }
}
