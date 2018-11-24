CREATE TABLE [logs].[SQLException](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[ProcedureName] [nvarchar](100) NOT NULL,
	[ErrorMessage] [nvarchar](4000) NULL,
	[ErrorNumber] [int] NULL,
	[ErrorLine] [int] NULL,
	[ErrorSeverity] [int] NULL,
	[ErrorState] [int] NULL,
	[ErrorDate] [datetime] NOT NULL,
 CONSTRAINT [PK_SQLException] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [logs].[SQLException] ADD  CONSTRAINT [DF_SQLException_ErrorDate]  DEFAULT (getutcdate()) FOR [ErrorDate]


GO

CREATE INDEX [IX_SQLException_ErrorDate] ON [logs].[SQLException] ([ErrorDate])
GO

CREATE INDEX [IX_SQLException_ProcedureName] ON [logs].[SQLException] ([ProcedureName])
