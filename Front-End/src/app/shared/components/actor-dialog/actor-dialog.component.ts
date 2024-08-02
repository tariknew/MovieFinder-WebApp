import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
@Component({
  selector: 'app-actor-dialog',
  templateUrl: './actor-dialog.component.html',
  styleUrls: ['./actor-dialog.component.css']
})
export class ActorDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {}

  getActorProfileImage(profileImage: string): string {
    return `data:image/jpeg;base64,${profileImage}`;
  }
}
