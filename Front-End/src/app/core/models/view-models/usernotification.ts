export interface UserNotification {
  isRead: boolean;
  notificationId: number;
  userId: number;
  notification: Notification;
  notificationText: string;
  notificationDate: string;
  movieID: number;
}
export interface Notification {
  notificationText: string;
  dateTime: string;
  creatorId: number;
}







