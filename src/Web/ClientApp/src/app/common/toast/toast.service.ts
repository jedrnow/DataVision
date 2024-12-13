import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({
  providedIn: 'root',
})
export class ToastService {
  constructor(private snackBar: MatSnackBar) {}

  private defaultConfig: MatSnackBarConfig = {
    duration: 3000, // Duration in milliseconds
    horizontalPosition: 'right', 
    verticalPosition: 'top',
  };

  show(message: string, action: string = 'Close', config: MatSnackBarConfig = this.defaultConfig): void {
    console.log(config);
    this.snackBar.open(message, action, config);
  }

  showSuccess(message: string): void {
    this.show(message, 'Close', { ...this.defaultConfig, panelClass: ['success-snack'] });
  }

  showError(message: string): void {
    this.show(message, 'Retry', { ...this.defaultConfig, panelClass: ['error-snack'] });
  }

  showInfo(message: string): void {
    this.show(message, 'OK', { ...this.defaultConfig, panelClass: ['info-snack'] });
  }
}
