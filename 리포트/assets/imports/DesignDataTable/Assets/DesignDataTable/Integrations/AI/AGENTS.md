# Design Data Table AI integration

Use this local integration for Design Data Table workbook and game-data edits. Read `capabilities.json`, or run `python ddt_cli.py --ai-info`. The same API is exposed through the stdio MCP server `ddt_mcp.py`.

1. Discover the source with `ddt_search_tables` / `tables.search`.
2. Inspect schema, Enum members and revision. Read only needed Ids, fields or A1 ranges.
3. Prepare one workbook batch with `dryRun: true`, the inspected `expectedRevision`, and an optional range preview. Inspect its diagnostics and image.
4. Commit the same actions with `dryRun: false` and the unchanged revision. Read back the result.

Do not bypass the API with direct ZIP/XML or generated-code changes. Do not guess column names, Enum values or revisions. Never blindly retry a timed-out write; inspect its request Id first. Unsaved user drafts block mutations. Unity must be open, idle and outside Play Mode. Transactions span one workbook, not several workbooks.

The CLI is dependency-free Python 3.10+. Use `--project <Unity project>` or `DDT_PROJECT`; a project cwd or installed integration location is also detected. Print a client-neutral absolute configuration with `python ddt_cli.py --project <project> --mcp-config --pretty`, then copy its `mcpServers` object into the client's documented settings. No client configuration is changed automatically.

MCP clients discover eight tools through tools/list; clients that support local agent plugins can load this directory. Cached plugins require DDT_PROJECT or the project working directory. Other agents must read these instructions or receive the MCP configuration. No integration can force an unconnected AI to discover or call tools.
