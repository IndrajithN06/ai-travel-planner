import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

export interface DeleteConfirmationData {
  title: string;
  message: string;
}

@Component({
  selector: 'app-delete-confirmation-dialog',
  standalone: true,
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule,
    MatIconModule
  ],
  template: `
    <h2 mat-dialog-title>
      <mat-icon color="warn">warning</mat-icon>
      Confirm Deletion
    </h2>
    <mat-dialog-content>
      <p class="confirmation-message">{{ data.message }}</p>
      <p class="item-title">{{ data.title }}</p>
    </mat-dialog-content>
    <mat-dialog-actions align="end">
      <button mat-button (click)="onCancel()">Cancel</button>
      <button mat-flat-button color="warn" (click)="onConfirm()">
        <mat-icon>delete</mat-icon>
        Delete
      </button>
    </mat-dialog-actions>
  `,
  styles: [`
    h2 {
      display: flex;
      align-items: center;
      gap: 12px;
      margin: 0;
      color: #2c3e50;
    }

    mat-dialog-content {
      padding: 24px;
      min-width: 300px;
    }

    .confirmation-message {
      margin: 0 0 16px 0;
      color: #5a6c7d;
      line-height: 1.5;
    }

    .item-title {
      margin: 0;
      font-weight: 600;
      color: #2c3e50;
      padding: 12px;
      background: #f8f9fa;
      border-radius: 8px;
      border-left: 4px solid #e74c3c;
    }

    mat-dialog-actions {
      padding: 16px 24px;
      margin: 0;

      button {
        display: flex;
        align-items: center;
        gap: 6px;
      }
    }

    @media (prefers-color-scheme: dark) {
      h2 {
        color: #ecf0f1;
      }

      .confirmation-message {
        color: #bdc3c7;
      }

      .item-title {
        color: #ecf0f1;
        background: #2c3e50;
      }
    }
  `]
})
export class DeleteConfirmationDialogComponent {
  constructor(
    public dialogRef: MatDialogRef<DeleteConfirmationDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DeleteConfirmationData
  ) {}

  onCancel(): void {
    this.dialogRef.close(false);
  }

  onConfirm(): void {
    this.dialogRef.close(true);
  }
}

